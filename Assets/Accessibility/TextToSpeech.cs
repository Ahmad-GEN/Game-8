using UnityEngine.UI;
//using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Newtonsoft;
using Newtonsoft.Json;

public class GetAudio
{
    public string mp3FileUrl { get; set; }
    public object timingFileUrl { get; set; }
    public List<int> timings { get; set; }
}

public class TextToSpeech : MonoBehaviour
{
    public int i = 1, cookieNum = 1;
    bool wait = false;
    //public bool n2y = false;
    AudioClip TempAudioClip;//By Humza
    public External ExternalRef;
    // private string key = "1246840dec93420a9e1f199bfdb3ae85"; //arslan
    // private string key = "d4856606d9de439aa37ce4d9e53ec0e1";//zaki
    [HideInInspector]
    public string key;//humza
    //private string key = "";
    public AudioSource source;
    private AudioClip clip;
    //public InputField inputText;
    public bool LocalTTS;//false for main n2y build , true for local build 
    string text;
    //public Queue<AudioClip> sound = new Queue<AudioClip>();
    public int TotalAudioToDownload;//total audio should be downloaded first before game is started
    [HideInInspector]
    public int AudioDownloaded;
    public static TextToSpeech ins;
    public bool justtext = false;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void OnEnable()
    {

        // playtext("Kindly navigate the game using buttons");
        if (GetComponent<AudioSource>() != null)
            source = GetComponent<AudioSource>();
        ins = this;
    }
    IEnumerator downloadAudio(AccessibiltyObject t)
    {
        if (LocalTTS)
        {

            Regex rg = new Regex("\\s+");
            string result = rg.Replace(text, "+");
            string url = "http://api.voicerss.org/?key=" + ExternalRef.KeyNew + "&hl=en-us&src=" + result + "&c=WAV";
            Debug.Log("Key Used : " + ExternalRef.KeyNew);
            WWW ww = new WWW(url);
            yield return ww;
            if (justtext)
            {
                playAudio(ww.GetAudioClip(false, false, AudioType.WAV));
            }
            else
            {
                if (t != null)
                    t.clip = ww.GetAudioClip(false, false, AudioType.WAV);
            }

        }
        else
        {
            Regex rg = new Regex("\\s+");
            string result = rg.Replace(text, "+");
            string url = ExternalRef.BaseUrl + "api/speechapi/GetDynamicSpeechData?text=" + result + "&speed=30&volume=90&speechLanguage=en";//n2y server
            WWW ww = new WWW(url);
            yield return ww;
            Debug.Log("Audio downloaded 2: " + ww.text);
            GetAudio Audio = JsonConvert.DeserializeObject<GetAudio>(ww.text);
            string NewUrl = Audio.mp3FileUrl;
            WWW wwNew = new WWW(NewUrl);
            yield return wwNew;
            Debug.Log("Done waiting now storing audio " + NewUrl + " " + wwNew.text + "  " + result);
            if (justtext)
                playAudio(wwNew.GetAudioClip(false, false, AudioType.MPEG));
            else
            {
                if (t != null)
                    t.clip = wwNew.GetAudioClip(false, false, AudioType.MPEG);
            }


            justtext = false;
        }

    }
    public void DownloadAllAudios(AccessibiltyObject t)
    {
        if (t.gameobject.GetComponent<TextToSpeak>())
        {
            text = t.gameobject.GetComponent<TextToSpeak>().textToSpeak;
        }
        else
        {
            text = t.gameobject.name;
        }
        Debug.Log("AudioPlaying1");
        StartCoroutine(downloadAudio(t));
    }
    public void playAudio(AudioClip clip)
    {
        if (clip != null)
        {
            // print("playing sound");
            source.clip = clip;
            source.Play();
        }
    }

    public void StopAudio()
    {
        Debug.Log("TextToSpeech->StopAudio(" + source.isPlaying + ")");

        if (source.isPlaying)
        {
            Debug.Log("if (true)");
            source.Stop();
        }
    }

    public void playAudioThroughText(string t)
    {
        justtext = true;
        text = t;
        Debug.Log("AudioPlaying2 : " + text);
        StartCoroutine(downloadAudio(null));
    }

    public void DowloadStartingSound(string[] text, List<AudioClip> clips)
    {
        // Debug.Log("The New Key is : " + key);
        clips.Clear();
        StartCoroutine(Download(text, clips));

    }
    public IEnumerator DownloadInbetweenSounds(string text)
    {
        if (LocalTTS)
        {
            Regex rg = new Regex("\\s+");
            string result = rg.Replace(text, "+");
            string url = "http://api.voicerss.org/?key=" + ExternalRef.KeyNew + "&hl=en-us&src=" + result + "&c=WAV";
            Debug.Log("Key Used : " + ExternalRef.KeyNew);
            WWW ww = new WWW(url);
            yield return ww;
            playAudio(ww.GetAudioClip(false, false, AudioType.WAV));

        }
        else
        {
            Regex rg = new Regex("\\s+");
            string result = rg.Replace(text, "+");
            string url = ExternalRef.BaseUrl + "api/speechapi/GetDynamicSpeechData?text=" + result + "&speed=30&volume=90&speechLanguage=en";//n2y server
            WWW ww = new WWW(url);
            yield return ww;
            Debug.Log("Audio downloaded : " + ww.text);
            GetAudio Audio = JsonConvert.DeserializeObject<GetAudio>(ww.text);
            string NewUrl = Audio.mp3FileUrl;
            WWW wwNew = new WWW(NewUrl);
            yield return wwNew;
            Debug.Log("Done waiting now storing audio " + NewUrl + " " + wwNew.text + "  " + result);
            playAudio(wwNew.GetAudioClip(false, false, AudioType.MPEG));
        }
    }

    IEnumerator Download(string[] text, List<AudioClip> clips)
    {
        if (LocalTTS)
        {
            if (googleTTSSettings.useGoogleTTS)
                GoogleTextToSpeech(text, clips);
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    Regex rg = new Regex("\\s+");
                    string result = rg.Replace(text[i], "+");
                    string url = "http://api.voicerss.org/?key=" + ExternalRef.KeyNew + "&hl=en-us&src=" + result + "&c=WAV";
                    Debug.Log("Key Used : " + ExternalRef.KeyNew);
                    WWW ww = new WWW(url);
                    yield return ww;
                    clips.Add(ww.GetAudioClip(false, false, AudioType.WAV));
                    AudioDownloaded++;
                    Debug.Log(AudioDownloaded);
                }
            }
        }
        else
        {
            for (int i = 0; i < text.Length; i++)
            {
                Regex rg = new Regex("\\s+");
                string result = rg.Replace(text[i], "+");
                string url = ExternalRef.BaseUrl + "api/speechapi/GetDynamicSpeechData?text=" + result + "&speed=30&volume=90&speechLanguage=en";//n2y server
                WWW ww = new WWW(url);
                yield return ww;
                Debug.Log("Audio downloaded: " + ww.text);
                GetAudio Audio = JsonConvert.DeserializeObject<GetAudio>(ww.text);
                string NewUrl = Audio.mp3FileUrl;
                WWW wwNew = new WWW(NewUrl);
                yield return wwNew;
                Debug.Log("Done waiting now storing audio " + NewUrl + " " + wwNew.text);
                clips.Add(wwNew.GetAudioClip(false, false, AudioType.MPEG));
                AudioDownloaded++;
                Debug.Log(AudioDownloaded);
            }
        }
    }

    #region Google Text to speech

    [Serializable]
    public class GoogleTTSSettings
    {
        [Serializable]
        public class DowloadedDataDetails
        {
            public int downloadedAudios;
            public int alreadyDownloadedAudios;
        }
        public DowloadedDataDetails dowloadedDataDetails;

        public bool useGoogleTTS = true;
        public bool isDownloaded;
        public string url = "https://texttospeech.googleapis.com/v1/text:synthesize?fields=audioContent&key=";
        public string path = "Assets/Resources/Sounds/";
        public string APIKey = "AIzaSyDbmWAwzxnVhXKVaGg7Gkb1d-67VwsvEoA";
    }

    public GoogleTTSSettings googleTTSSettings;
    string url;
    string bodyData;

    public void GoogleTextToSpeech(string[] text, List<AudioClip> clips)
    {
        // url = "https://texttospeech.googleapis.com/v1/text:synthesize?fields=audioContent&key=" + APIKey;
        url = googleTTSSettings.url + googleTTSSettings.APIKey;

        for (int i = 0; i < text.Length; i++)
        {
            if (!googleTTSSettings.isDownloaded)
            {
                if (ReturnClipofName(text[i]) == null)
                {
                    bodyData = "{\"audioConfig\": {\"pitch\": 0,\"audioEncoding\": \"MP3\",\"speakingRate\": 1},\"input\": {\"ssml\": \"<speak>" + text[i] + "</speak>\"},\"voice\": {\"languageCode\": \"en-US\",\"name\": \"en-US-Wavenet-E\", \"ssmlGender\":\"FEMALE\"}}";
                    StartCoroutine(PostRequest(url, bodyData, text[i]));
                }
                else
                {
                    Debug.Log("\"" + text[i] + "\" is already available.");
                    googleTTSSettings.dowloadedDataDetails.alreadyDownloadedAudios++;
                }
            }
            else
                clips.Add(ReturnClipofName(text[i]));
        }
    }

    IEnumerator PostRequest(string url, string json, string audioName)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("Request for \"" + audioName + "\" is sent.");
        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            // Debug.Log("Received: " + uwr.downloadHandler.text);

            string base64Text = uwr.downloadHandler.text;
            MakeAudioClip(base64Text.Substring(21, base64Text.Length - 25), audioName);
            Debug.Log("Request for \"" + audioName + "\" is done.");
            googleTTSSettings.dowloadedDataDetails.downloadedAudios++;
        }
    }

    void MakeAudioClip(string base64Text, string audioName)
    {
        MakeDirectoryIfNotExists();
        byte[] bytes = System.Convert.FromBase64String(base64Text);
        File.WriteAllBytes(googleTTSSettings.path + audioName + ".wav", bytes);
    }

    AudioClip ReturnClipofName(string audioName)
    {
        MakeDirectoryIfNotExists();
        AudioDownloaded++;
        return Resources.Load<AudioClip>("Sounds/" + audioName);
    }

    void MakeDirectoryIfNotExists()
    {
        if (!Directory.Exists("Assets/Resources/Sounds"))
            Directory.CreateDirectory("Assets/Resources/Sounds");
    }

    #endregion
    public void playLongAudio(int i, int cookieNum)
    {
        if (GameManager.Instance.Accessibilty)
        {
            if (wait == false)
                StartCoroutine(playIT(i, cookieNum));
        }
    }

    public void playLongAudio()
    {
        if (GameManager.Instance.Accessibilty)
        {
            if (wait == false)
                StartCoroutine(playIT(i, cookieNum));
        }
    }

    IEnumerator playIT(int i, int cookieNum)
    {
        yield return new WaitForSeconds(0.1f);
        // float gap = 0.75f;
        // wait = true;
        // AccessibilityManager.instance.ToggleNaviagtion(false); //arslan
        // yield return new WaitForSeconds(01);
        // //kindly select
        // source.clip = AudioFile.instance.others[0];
        // source.Play();
        // yield return new WaitForSeconds(source.clip.length-gap);
        // //num of cookie

        // source.clip = AudioFile.instance.numbers[i];
        // source.Play();
        // yield return new WaitForSeconds(source.clip.length-gap);
        // //cookie nam
        // if (i == 0)
        // {
        //     source.clip = AudioFile.instance.cookie[cookieNum];
        //     source.Play();
        //     yield return new WaitForSeconds(source.clip.length-gap);
        // }
        // else
        // {
        //     source.clip = AudioFile.instance.cookies[cookieNum];
        //     source.Play();
        //     yield return new WaitForSeconds(source.clip.length-gap);
        // }
        // AccessibilityManager.instance.ToggleNaviagtion(true); //arslan
        // wait = false;
    }

    //	IEnumerator SaveSound(WWW m_www, string Filename) //By Humza
    //	{
    //		//yield return new WaitForSeconds(1);
    //
    //		if (m_www != null && m_www.isDone && m_www.error == null)
    //		{
    //			TempAudioClip = m_www.GetAudioClip(false, false, AudioType.WAV);
    //			//print("m_www: " + m_www.bytesDownloaded);
    //			System.IO.File.WriteAllBytes(Application.persistentDataPath + "/"+Filename+".wav",m_www.bytes);
    //			//Debug.Log ("file saved in : " + Application.persistentDataPath + "/" + Filename + ".wav");
    //		}
    //
    //		yield return null;
    //	}

    //	public void Load(List<AudioClip> clips,string name, int ArrayLength)
    //	{
    //		StartCoroutine (LoadFromMemory (clips, name,ArrayLength));
    //	}
    //
    //	IEnumerator LoadFromMemory(List<AudioClip> clips,string Filename, int Length)//By Humza
    //	{
    //		for (int i = 0; i < Length; i++) {
    //			
    //			string path = Application.persistentDataPath + "/"+Filename+i.ToString()+".wav";
    ////			Debug.Log (path);
    //			WWW www = new WWW("file://"+path);
    //
    //			if (File.Exists(path))
    //			{
    //				yield return www;
    //				TempAudioClip = www.GetAudioClip(false, false, AudioType.WAV);
    //				clips.Add(TempAudioClip);
    //			}
    //			else
    //			{
    //				Debug.Log ("path null");
    //			}
    //			
    //		}
    //
    //
    //	}
}