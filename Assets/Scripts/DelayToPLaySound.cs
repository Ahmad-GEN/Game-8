using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DelayToPLaySound : MonoBehaviour
{

    public int AudioToPlay;
    GameManager Sound;
    public float sec = 1f;

    // Use this for initialization

    void OnEnable()
    {
        Sound = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(wait());
    }
    // Update is called once per frame
    void Update()
    {

    }

    float[] sentencesLengths = { };
    // Use to check which audio index have multiple sentences
    int magicIndex = -1;

    IEnumerator wait()
    {
        yield return new WaitForSeconds(sec);

        Debug.Log("DelayToPlaySound->wait(" + AudioToPlay + ")");
        Sound.Audio.clip = Sound.Sounds[AudioToPlay];
        Sound.Audio.Play();

        if (GameManager.Instance.GameNumber(1))
        {
			magicIndex = 26;
			sentencesLengths = new float[8];
            Generic<float>.FillArray(sentencesLengths, 2.55f, 2.28f, 2.45f, 4.32f, 3.14f, 4.58f, 1.92f, 2.23f);
        }
		else if (GameManager.Instance.GameNumber(2))
        {
			magicIndex = 20;
			sentencesLengths = new float[6];
            Generic<float>.FillArray(sentencesLengths, 2.56f, 1.72f, 3.35f, 1.64f, 1.64f, 1.86f);
        }
        else if (GameManager.Instance.GameNumber(3))
        {
			magicIndex = 20;
			sentencesLengths = new float[7];
            Generic<float>.FillArray(sentencesLengths, 2.10f, 2.38f, 4.98f, 2.61f, 3.93f, 1.52f, 1.99f);
        }
        else if (GameManager.Instance.GameNumber(4))
        {
			magicIndex = 20;
			sentencesLengths = new float[5];
            Generic<float>.FillArray(sentencesLengths, 1.60f, 2.92f, 2.99f, 1.41f, 2.02f);
        }
        else if (GameManager.Instance.GameNumber(5))
        {
			magicIndex = 20;
			sentencesLengths = new float[8];
            Generic<float>.FillArray(sentencesLengths, 3.33f, 3.86f, 2.28f, 3.01f, 4.16f, 4.16f, 1.16f, 1.73f);
        }

        // Because these sounds contains many sentences so printing each sentence seperatly in Closed Caption
        if (AudioToPlay == magicIndex)
        {
            string[] sentences = CloseCaption.CCManager.instance.seqStrings[AudioToPlay].Split('/');
            Debug.Log(sentences.Length);
            for (int i = 0; i < sentences.Length; i++)
            {
                CloseCaption.CCManager.instance.CreateCaption(sentences[i], sentencesLengths[i]);
                yield return new WaitForSeconds(sentencesLengths[i]);
            }
        }
        else
            CloseCaption.CCManager.instance.CreateCaption(AudioToPlay, Sound.Audio.clip.length);
    }

    public class Generic<DataType>
    {
        public static void FillArray(DataType[] array, params DataType[] values)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = values[i];
            }
        }
    }
}
