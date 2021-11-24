using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFile : MonoBehaviour
{

    public string[] inGameString;
    public string[] MainMenuString;
    public string[] destinationString;

    public string[] panelOneString;

    public string[] panelTwoString;

    public string[] panelThreeString;

    public string[] panelFourString;
    public string[] endingString;
    public string[] pauseString;
    public string[] othersString;

	[HideInInspector]
    public List<AudioClip> waitText, MainMenu, destination, panelOne, panelTwo, panelThree, panelFour, ending, others, pause;

    [HideInInspector]
    public static AudioFile instance;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // print("audiofile");
        instance = this;
        download();
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
	{
		//PlayerPrefs.SetString ("LoadFromCache", "YES");
		//PlayerPrefs.DeleteAll ();
    }
    public void download()
    {
		if (TextToSpeech.ins != null) {
			Debug.Log ("Downloading sounds");
			TextToSpeech.ins.DowloadStartingSound (MainMenuString, MainMenu);
			TextToSpeech.ins.DowloadStartingSound (inGameString, AccessibilityManager.instance.clips);
			TextToSpeech.ins.DowloadStartingSound (inGameString, AccessibilityManager.instance.infolips);
			TextToSpeech.ins.DowloadStartingSound (panelOneString, panelOne);
			TextToSpeech.ins.DowloadStartingSound (panelTwoString, panelTwo);
			TextToSpeech.ins.DowloadStartingSound (panelThreeString, panelThree);
			TextToSpeech.ins.DowloadStartingSound (panelFourString, panelFour);
			TextToSpeech.ins.DowloadStartingSound (destinationString, destination);
			TextToSpeech.ins.DowloadStartingSound (othersString, others);
			TextToSpeech.ins.DowloadStartingSound (endingString, ending);
			TextToSpeech.ins.DowloadStartingSound (pauseString, pause);
//			PlayerPrefs.SetString ("LoadFromCache", "YES");
		}
//		#endif
//
//    }
}
}