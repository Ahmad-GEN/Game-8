using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public class AccessibiltyObject // 
{

    public GameObject gameobject;
    public AudioClip clip;
    public string text;
}
public class listhandler : MonoBehaviour, AcessibilityInterface
{
    public string uniqueText = "ending";
    public string textToSayOnWait;
    public int soundIndex;
    public int timeToWaitToPlayTextATStart;
    public bool EnableAccessability = false;
    public GameObject[] extraResource;
    public List<GameObject> list;
    public List<AccessibiltyObject> accesabilityObject;
    public GameObject greenbox;
    RectTransform greenTransform;
    int currentListIndex = 0;
    bool enableTouch = false;
    bool isButton = false;
    GameObject buttonObject;

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        //  AccessibilityManager.ins.levelCompleted -= levelcompleted;

        // accesabilityObject.Clear();
        currentListIndex = 0;
        isButton = false;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
	void WaitToDisable()
    {
        if (AccessibilityManager.instance != null)
        {
            AccessibilityManager.instance.CanCheckInfo = false;
            // Debug.Log ("Disabled");
        }
    }
    void OnEnable()
    {
        Invoke("WaitToDisable", 2f);
        Invoke("WaitToEnable", 8f);
        currentListIndex = 0;

        greenTransform = greenbox.GetComponent<RectTransform>();
        GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
    }


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void Awake()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        // print(GameManager.Instance.Accessibilty);
        if (GameManager.Instance.Accessibilty)
        {

            AccessibilityManager.instance.populateAccessibiltyList(accesabilityObject, list, uniqueText);

        }
    }


    void GreenBoxNaviagtionAndAudio(RectTransform green, GameObject gameObject)
    {
        if (gameObject.GetComponent<Button>() != null) // check if the object has button then in order to perform its event we will add a boolen
        {
            isButton = true;
            buttonObject = gameObject;
        }
        else
        {
            isButton = false;
            buttonObject = null;
        }

        RectTransform list = gameObject.GetComponent<RectTransform>();
        if (green != null)
        {
            green.SetParent(list.parent);
            //green.parent = list.parent;
            green.anchorMin = list.anchorMin;
            green.anchorMax = list.anchorMax;
            green.anchoredPosition = list.anchoredPosition;
            green.sizeDelta = list.sizeDelta;
            green.eulerAngles = list.eulerAngles;
            enableTouch = true;
        }



    }
    public void changeState(bool state)
    {
        Debug.Log("changing state of  " + gameObject.name + "    " + state);
        EnableAccessability = false;

        if (state)
        {
            Invoke("wait", timeToWaitToPlayTextATStart);

        }
    }
    public void revertOption()
    {

    }
    void playsound(GameObject gameobject)
    {
        AccessibiltyObject tempAccessibiltyObject = accesabilityObject.Find(y => y.gameobject == gameobject);
        if (tempAccessibiltyObject != null)// will call to play sound by find object in accessability 
        {
            Debug.Log("Playing Sound");

            TextToSpeech.ins.playAudio(tempAccessibiltyObject.clip);
            CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(tempAccessibiltyObject.text), tempAccessibiltyObject.clip.length);
        }
    }
    public void moveForward()
    {
        if (EnableAccessability)
        {
            if (currentListIndex > 0)
            {
                Debug.Log("up " + currentListIndex);
                currentListIndex--;
                playsound(list[currentListIndex]);
                GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
            }
            else
            {
                currentListIndex = list.Count - 1;
                playsound(list[currentListIndex]);
                print("the length is  " + list.Count + "cuur " + currentListIndex);
                GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
            }
        }

    }
    public void moveBackward()
    {
        if (EnableAccessability)
        {

            if (currentListIndex < list.Count - 1)
            {
                currentListIndex++;
                playsound(list[currentListIndex]);
                GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
            }
            else
            {
                currentListIndex = 0;
                playsound(list[currentListIndex]);
                GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
            }
        }
    }
    public void select()
    {
        if (EnableAccessability)
        {
            if (isButton)
            {
                Debug.Log("touch of " + gameObject.name);
                isButton = false;
                buttonObject.GetComponent<Button>().onClick.Invoke();
                AccessibilityManager.instance.SwitchToNextState(null);
            }
        }
    }
    public void infoText()
    {
        if (TextToSpeech.ins != null)
        {
            if (soundIndex != 7)
            { //sound 7 is an empty sound used in some cases for this game
                TextToSpeech.ins.playAudio(AccessibilityManager.instance.infolips[soundIndex]);
                CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.infolips[soundIndex].length);
                Debug.Log("Index : " + soundIndex);
            }
        }
    }
    public void unselect()
    {

    }
    public string getWaitText()
    {
        return textToSayOnWait;
    }
    public void toggleNavigation(bool state)
    {
        EnableAccessability = state;
    }
    void wait()
    {
        float delayToEnableAccessibility = 0;
        // yield return new WaitForSeconds(timeToWaitToPlayTextATStart);
        if (AccessibilityManager.instance != null)
        {
            if (AccessibilityManager.instance.clips.Count > 0)
            {
                if (soundIndex != 7)
                { //sound 7 is an empty sound used in some cases for this game

                    if (AccessibilityManager.instance.gameWasPaused)
                    {
                        AccessibilityManager.instance.gameWasPaused = false;
                        StopAllCoroutines();
                        FreezeControlsHandler.Instance.FreezeControlls();
                        TextToSpeech.ins.playAudio(AccessibilityManager.instance.clips[soundIndex]);
                        CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.clips[soundIndex].length);
                        delayToEnableAccessibility = AccessibilityManager.instance.clips[soundIndex].length;
                    }
                    else
                    {
                        Debug.Log("Index2 : " + soundIndex);
                        FreezeControlsHandler.Instance.FreezeControlls();
                        TextToSpeech.ins.playAudio(AccessibilityManager.instance.clips[soundIndex]);
                        CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.clips[soundIndex].length);
                        delayToEnableAccessibility = AccessibilityManager.instance.clips[soundIndex].length;
                    }
                }
            }

            Invoke("waitE", delayToEnableAccessibility);
        }
    }

    void waitE()
    {
        Debug.Log("listHandler(" + gameObject.name + ")->waitE()");

        if (soundIndex != 7 || uniqueText.Equals("ending"))
            FreezeControlsHandler.Instance.UnFreezeControlls();

        EnableAccessability = true;

        greenbox.SetActive(true);
    }

    void WaitToEnable()
    {
        if (AccessibilityManager.instance != null)
        {
            AccessibilityManager.instance.checkActivity = true;
            AccessibilityManager.instance.CanCheckInfo = true;
            Debug.Log("made wait text true");
        }
    }

    #region UpdatedAccessibility
    public void move_with_click(GameObject obj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (obj.name == list[currentListIndex].name)
            {
                break;
            }
            moveForward();
        }
    }
    #endregion
}
