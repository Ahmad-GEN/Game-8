using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void _OnGameStarted();
    [DllImport("__Internal")]
    private static extern void _OnGameStopped();
    [DllImport("__Internal")]
    private static extern void _ExitFullScreen();
    [DllImport("_Internal")]
    private static extern bool _IsMobile();
    public int gameNumber;
    public float versionNumber;
    public bool unityLogger;
    public GameObject[] TaskPanel;
    public GameObject[] Panels_List;
    bool CanClickPlayOnce = true;
    public AudioClip[] Sounds;
    [HideInInspector]
    public AudioSource Audio;
    [HideInInspector]
    public Fade[] mode;
    public GameObject Pause_Pnl;
    public float DelayToStartGame;
    public int SoundToPlayStart;
    bool CanClickAgain = true;
    public string SceneName;
    public float SecondsToDelayAgain = 1.2f;
    [HideInInspector]
    public int LevelIndex = 0;
    //By Aleem
    public GameObject PlaqueScreen;
    public GameObject Again;
    public GameObject Stop;
    [HideInInspector]
    public int CurrentLevelIndex = 1;
    public static GameManager Instance;
    public bool Accessibilty = false;
    public GameObject AccessibiltyObject;
    public float gameSpeed = 1;
    public bool changeGameSpeed;
    public int pauseStopPressedCount;
    public bool graphicsCheck = false;
    public bool lowGraphics = false;
    public bool isMobile = false;
    public Text versionTxt;
    public GameObject block;

    [Tooltip("Fetched in Awake")]
    public BloomControl BloomController;
    public Material buttonGlowMat;
    public Material imgGlowMat;


    private void Awake()
    {
        BloomController = FindObjectOfType<BloomControl>();
        if (versionTxt)
        {
            versionTxt.text = versionNumber.ToString();
        }

    }

    void OnEnable()
    {
        //		if (Accessibilty) {
        //			AccessibiltyObject.SetActive (true);
        //		}

        // print("gameobj");
        print("Game Number: " + gameNumber);
        print("Version Number: " + versionNumber);
        Debug.unityLogger.logEnabled = unityLogger;// Removing Logs according to Code maintenance

        try
        {
            _OnGameStarted();
        }
        catch (Exception e) { }
    }
    // Use this for initialization
    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        _OnGameStarted();
        isMobile = _IsMobile();
        
#endif

       
        Instance = this;

        CheckLowGraphics(isMobile || IsWebGL1());
        pauseStopPressedCount = 0;
        PlayerPrefs.SetInt("FullScreen", 0);
        PlayerPrefs.SetInt("VoiceOvers", 0);           //on every game starts clear out the preferences
        PlayerPrefs.SetInt("AmountCollected", 0);
        PlayerPrefs.SetInt("EnvelopesPlaced", 0);
        //on every game starts clear out the preferences

        Audio = GetComponent<AudioSource>();
        for (int i = 0; i < Panels_List.Length; i++)
        {
            mode[i] = Panels_List[i].GetComponent<Fade>();
        }
    }

    public void CheckLowGraphics(bool _value)
    {
        lowGraphics = _value;
        if (lowGraphics)
        {
            GameManager.Instance.block.SetActive(true);
            buttonGlowMat = null;
            imgGlowMat = null;
        }
        graphicsCheck = true;
    }

    void Update()
    {
        if(changeGameSpeed)
        {
            changeGameSpeed = false;
            #if UNITY_EDITOR //for testing purposes in editor
            Time.timeScale = gameSpeed;
            #endif
        }
    }

    public IEnumerator WaittoQuit()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public void FadeOnClickBtn(int fadeOn)      //this is the function which will be called when play button and each next button at end of level is clicked 
    {
        if (fadeOn == 0 && CanClickPlayOnce)
        {
            if (AccessibilityManager.instance != null)
            {
                AccessibilityManager.instance.CanCheckInfo = false;
            }
            Audio.clip = Sounds[SoundToPlayStart];
            Debug.Log("GameManager->FadeOnClickBtn(" + SoundToPlayStart + ")");
            CloseCaption.CCManager.instance.CreateCaption(SoundToPlayStart, Audio.clip.length);
            Audio.Play();


            CanClickPlayOnce = false;
            StartCoroutine(WaitToDisable(DelayToStartGame, fadeOn));
        }
        else if (fadeOn != 0)
        {
            TaskPanel[fadeOn].SetActive(true);
            OnTriggerenter triggerHandler = GameObject.FindGameObjectWithTag("Collector").GetComponent<OnTriggerenter>();
            triggerHandler.LevelEnd();
            StartCoroutine(WaitToDisable(6.85f, fadeOn));
        }
    }

    public void OnButtonClicked(string BtnName)  //called when UI buttons are preesed
    {
        if (BtnName == "Again" && CanClickAgain)
        {
            Time.timeScale = 1f;
            CanClickAgain = false;
            StartCoroutine(LoadScene());
        }
        else if (BtnName == "Stop")
        {
            
            if (Screen.fullScreen)
            {
                _ExitFullScreen();
                Screen.fullScreen = !Screen.fullScreen;
            }
            //SceneManager.LoadScene (SceneName, LoadSceneMode.Single);
            
            if (!External.Instance.Preview)
                StartCoroutine(CallStop());
            else
                _OnGameStopped();
            //Application.Quit ();
        }
        else if (BtnName == "FullScrren")
        {
            if (Screen.fullScreen)
            {
                _ExitFullScreen();
                Screen.fullScreen = false;
                FullScreenBtn.Instance.IMG.sprite = FullScreenBtn.Instance.FullScreenIMG[0];
            }
            else
            {
                FullScreenBtn.Instance.IMG.sprite = FullScreenBtn.Instance.FullScreenIMG[1];
                Screen.fullScreen = true;
            }
        }
        else if (BtnName == "Pause")
        {
            Time.timeScale = 0f;
            Pause_Pnl.SetActive(true);
        }
        else if (BtnName == "PausePlay")
        {
            if (AccessibilityManager.instance != null)
            {
                AccessibilityManager.instance.ShowPausePanel = true;
                //AccessibilityManager.instance.gameWasPaused = true;
            }
            Time.timeScale = 1f;
            Pause_Pnl.SetActive(false);
        }
        else if (BtnName == "PauseStop")
        {
            // So that player can press stop button on pause screen only once
            if(pauseStopPressedCount < 1)
                pauseStopPressedCount++;
            else return;

            if (AccessibilityManager.instance != null)
            {
                AccessibilityManager.instance.LastCheck = false;
            }
            if (EventController.instance != null && !External.Instance.Preview)
            {
                EventController.instance.CountScreenInteractionWithoutCheck();
                EventController.instance.currentGamePercentage();
            }

            Time.timeScale = 1f;
            Debug.Log("Ending the game");

            if (Screen.fullScreen)
            {
                _ExitFullScreen();
                Screen.fullScreen = !Screen.fullScreen;
            }

            if (EventController.instance != null && !External.Instance.Preview)
            {
                StartCoroutine(CallStop());
            }
            else
            {
                _OnGameStopped();
            }
            //SceneManager.LoadScene (SceneName, LoadSceneMode.Single);
        }
    }

    IEnumerator WaitToDisable(float Sec, int FadeOn)
    {
        yield return new WaitForSeconds(Sec);
        if (FadeOn == 0)
        {
            mode[0].Fadeout = true;
            yield return new WaitForSeconds(1.2f);
            TaskPanel[FadeOn].SetActive(true);
            Panels_List[FadeOn].SetActive(false);
            //SceneManager.LoadScene (1, LoadSceneMode.Single);
        }
        else
        {
            Panels_List[FadeOn].SetActive(false);
        }
    }

    public IEnumerator NextLevel(int Ind)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < Panels_List.Length; i++)
        {
            if (i == Ind)
            {
                Panels_List[i].SetActive(true);
            }
            else
            {
                Panels_List[i].SetActive(false);
            }
        }
    }
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(SecondsToDelayAgain);
        if (Screen.fullScreen)
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public void NextScene()
    {
        if (CanClickPlayOnce)
        {
            Audio.clip = Sounds[SoundToPlayStart];
            Debug.Log("GameManager->FadeOnClickBtn(" + SoundToPlayStart + ")");
            CloseCaption.CCManager.instance.CreateCaption(SoundToPlayStart, Audio.clip.length);
            Audio.Play();

            CanClickPlayOnce = false;
            StartCoroutine(WaitToDisable(DelayToStartGame, 0));
        }
    }

    public void GoToSelectionScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public IEnumerator CallStop()
    {
         yield return new WaitUntil(() => RestAPIHandler.Instance.UploadedSuccessfully == true);
        _OnGameStopped();
    }

    public bool GameNumber(params int[] gameNumbers)
    {
        Debug.Log("GameManager->GameNumber()");

        for (int i = 0; i < gameNumbers.Length; i++)
        {
            if (gameNumbers[i] == gameNumber)
                return true;
        }

        return false;
    }

    public bool IsWebGL1()
    {
        if (SystemInfo.graphicsDeviceType.Equals(GraphicsDeviceType.OpenGLES3))//if (SystemInfo.graphicsDeviceType.Equals((GraphicsDeviceType)(11)))
        {
            //Debug.Log("Graphics Device Type is OPENGLES3");
            return false;
        }
        else if (SystemInfo.graphicsDeviceType.Equals(GraphicsDeviceType.OpenGLES2))
        {
            //Debug.Log("Graphics Device Type is OPENGLES2");
            return true;
        }
        else
        {
            //Debug.Log("Returning False since GraphicsDeviceType didnt match WebGl2 or Webgl1");
            return false;
        }

    }
}