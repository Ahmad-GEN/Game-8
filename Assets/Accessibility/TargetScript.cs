using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour, AcessibilityInterface
{
    public string uniqueText = "panelOne";
    public int timeToWaitToPlayTextATStart = 3;
    public int soundIndex = 2;
    public string textToSayOnWait;
    public bool removeTarget = false;
    public bool EnableAccessability = false;
    public List<GameObject> list;
    public List<AccessibiltyObject> accesabilityObject;
    public GameObject greenbox;
    RectTransform greenTransform;
    int currentListIndex = 0;
    bool enableTouch = false;
    bool isButton = false;
    GameObject buttonObject;
    GameObject tempObject;

    [SerializeField] private float minimumSwipeDistanceY;
    [SerializeField] private float minimumSwipeDistanceX;

    private Touch t = default(Touch);
    private Vector3 startPosition = Vector3.zero;

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        if (GameManager.Instance.Accessibilty)
        {
            print("Going to disable the target script for object " + gameObject.name);

            accesabilityObject.Clear();
            currentListIndex = 0;
            isButton = false;
        }

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void OnEnable()
    {
        if (GameManager.Instance.Accessibilty)
        {
            AccessibilityManager.instance.enablepause = true;
            Debug.Log("##########################################");
            currentListIndex = 0;

            greenTransform = greenbox.GetComponent<RectTransform>();

            GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);

        }
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void Awake()
    {
        GameObject obj;
        if (GameManager.Instance.Accessibilty)
        {
            // for (int i = 0; i < transform.childCount; i++)
            // {
            //     obj = transform.GetChild(i).gameObject;
            //     list.Add(obj);
            //     if (i == 0)
            //     {
            //         obj.GetComponent<TextToSpeak>().textToSpeak = "sprinkle cookie";
            //     }
            //     else if (i == 1)
            //     {
            //         obj.GetComponent<TextToSpeak>().textToSpeak = "chocolate chip cookie";
            //     }
            //     else if (i == 2)
            //     {
            //         obj.GetComponent<TextToSpeak>().textToSpeak = "striped cookie";
            //     }
            //     else if (i == 3)
            //     {
            //         obj.GetComponent<TextToSpeak>().textToSpeak = "chocolate cookie";
            //     }
            //     else if (i == 4)
            //     {
            //         obj.GetComponent<TextToSpeak>().textToSpeak = "jelly cookie";
            //     }
            //     else if (i == 5)
            //     {
            //         obj.GetComponent<TextToSpeak>().textToSpeak = "sugar cookie";
            //     }
            // }
            AccessibilityManager.instance.populateAccessibiltyList(accesabilityObject, list, uniqueText);
        }


    }

    void GreenBoxNavigationAndAudio(RectTransform green, GameObject gameObject)
    {

        if (gameObject.GetComponent<Button>()) // check if the object has button then in order to perform its event we will add a boolen
        {
            print("button found");
            isButton = true;
            buttonObject = gameObject;
        }
        else
        {
            isButton = false;
            buttonObject = null;
        }
        //  TextToSpeech.ins.onclick(gameObject.name);
        //   yield return new WaitForSeconds(0.5f);
        // Debug.Log(gameObject.name);
        // Debug.Log(green.name);
        // Debug.Log(green.parent.name);
        RectTransform list = gameObject.GetComponent<RectTransform>();
        green.parent = list.parent;
        //    green.SetSiblingIndex(0);
        green.anchorMin = list.anchorMin;
        green.anchorMax = list.anchorMax;
        green.anchoredPosition = list.anchoredPosition;
        green.sizeDelta = list.sizeDelta;
        green.eulerAngles = list.eulerAngles;
        enableTouch = true;
    }


    public void revertOption()
    {
        tempObject.SetActive(true);
        // list.Add(tempObject);
    }
    public void changeState(bool state)
    {
        greenTransform = greenbox.GetComponent<RectTransform>();
        EnableAccessability = false;
        currentListIndex = list.Count - 1;
        Debug.Log("changing state of  " + gameObject.name + "    " + state + " count  " + list.Count.ToString());
        if (list.Count > 0 && state)
        {
            // TextToSpeech.ins.playtext("navigate through target");
            GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
            //TextToSpeech.ins.playAudio (AccessibilityManager.instance.clips [soundIndex]);
            Invoke("wait", timeToWaitToPlayTextATStart);
        }


        // StartCoroutine(delay(state));

    }
    // IEnumerator delay(bool state)
    // {
    //     yield return new WaitForSeconds(3);
    //     greenTransform = greenbox.GetComponent<RectTransform>();
    //     EnableAccessability = false;
    //     currentListIndex = list.Count - 1;
    //     Debug.Log("changing state of  " + gameObject.name + "    " + state + " count  " + list.Count.ToString());
    //     if (list.Count > 0 && state)
    //     {
    //         // TextToSpeech.ins.playtext("navigate through target");
    //         GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
    //         StartCoroutine(wait());
    //     }
    // }
    void playsound(GameObject gameObject)
    {
        AccessibiltyObject tempAccessibiltyObject = accesabilityObject.Find(y => y.gameobject == gameObject);
        if (tempAccessibiltyObject != null)// will call to play sound by find object in accessability 
        {
            // TextToSpeech.ins.playAudio(accesabilityObject.Find(y => y.gameobject == gameObject).clip);
            Debug.Log("Playing Sound");

            TextToSpeech.ins.playAudio(tempAccessibiltyObject.clip);
            CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(tempAccessibiltyObject.text), tempAccessibiltyObject.clip.length);
        }
    }

    AudioClip GetSound(GameObject gameObject)
    {
        if (accesabilityObject.Exists(x => x.gameobject == gameObject))// will call to play sound by find object in accessability
            return accesabilityObject.Find(y => y.gameobject == gameObject).clip;

        return null;
    }

    string GetText(GameObject gameObject)
    {
        if (accesabilityObject.Exists(x => x.gameobject == gameObject))// will call to play sound by find object in accessability
            return accesabilityObject.Find(y => y.gameobject == gameObject).text;

        return null;
    }

    public void moveForward()
    {
        if (EnableAccessability)
        {
            if (currentListIndex > 0)
            {
                // Debug.Log("up " + currentListIndex);
                //StartCoroutine(CopyTransform(greenTransform, list[currentListIndex - 1]));
                currentListIndex--;
                if (!list[currentListIndex].activeSelf)
                {
                    moveForward();
                }
                else
                {
                    playsound(list[currentListIndex]);
                    GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
                }
            }
            else
            {
                currentListIndex = list.Count - 1;
                if (!list[currentListIndex].activeSelf)
                {
                    moveForward();
                }
                else
                {
                    playsound(list[currentListIndex]);
                    GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
                }
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
                if (!list[currentListIndex].activeSelf)
                {
                    moveBackward();
                }
                else
                {
                    playsound(list[currentListIndex]);
                    GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
                }
            }
            else
            {
                currentListIndex = 0;
                if (!list[currentListIndex].activeSelf)
                {
                    moveBackward();
                }
                else
                {
                    playsound(list[currentListIndex]);
                    GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
                }
            }
        }
    }
    public void select()
    {
        Debug.Log("I am in Select() function of Target");
        if (EnableAccessability)
        {
            //FreezeControlsHandler.Instance.FreezeControlls();
            if (AccessibilityManager.instance.DragnDrop)
            {
                print("drag n drop");
                //Invoke("EnableAfterDelay", 1f);
                if (removeTarget)
                {
                    Debug.Log("curr index is  " + currentListIndex + " length is " + list.Count + " obj is " + gameObject.name);
                    if (list != null && list[currentListIndex].activeSelf)
                    {
                        tempObject = list[currentListIndex];
                        list[currentListIndex].SetActive(false);
                        list.RemoveAt(currentListIndex);
                        if (list.Count > 0)
                        {
                            currentListIndex = 0;
                            playsound(list[currentListIndex]);
                            GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
                        }
                        if (AccessibilityManager.instance.isCleanUP || GameManager.Instance.gameNumber == 2)
                        {
                            AccessibilityManager.instance.AfterDestinationSelected(tempObject);
                        }
                        else
                        { 
                            AccessibilityManager.instance.SwitchToNextState(tempObject); 
                        }
                    }
                    else
                    {
                        Debug.Log("list is empty" + list.Count);
                    }
                }
                tempObject = list[currentListIndex];
                AccessibilityManager.instance.AfterDestinationSelected(tempObject);

                //else
                //{
                //    print("simple");

                //    if (!isButton)
                //        return;

                //    buttonObject.GetComponent<Button>().onClick.Invoke();

                //    isButton = false;

                //    tempObject = list[currentListIndex];
                //    if (!AccessibilityManager.instance.isSingleDestination)// gets called when game only has 1 destination and we will not need destination.
                //    {
                //        AccessibilityManager.instance.AfterDestinationSelected(tempObject);
                //    }
                //}


                ////AccessibilityManager.instance.SwitchToNextState(tempObject);
                ////  TextToSpeech.ins.playAudioThroughText(tempObject.GetComponent<TextToSpeak>().textToSpeak + " selected");
            }
            else
            {
                print("simple");
                if (isButton)
                {
                    Debug.Log("touch of " + gameObject.name);
                    isButton = false;
                    buttonObject.GetComponent<Button>().onClick.Invoke();
                    GreenBoxNavigationAndAudio(greenTransform, list[currentListIndex]);
                    if (buttonObject.GetComponent<TextToSpeak>())
                    {
                        if (buttonObject.GetComponent<TextToSpeak>().changeState)
                        {
                            AccessibilityManager.instance.SwitchToNextState(null);
                        }
                    }
                }
            }
        }


    }
    public void unselect()
    {
        if (EnableAccessability)
        {
            AccessibilityManager.instance.revertBackToTarget();
        }

    }
    public void infoText()
    {
        //  TextToSpeech.ins.playAudio(AccessibilityManager.instance.infolips[soundIndex]);
        if (EnableAccessability)
        {
            // TextToSpeech.ins.playLongAudio();
            if (soundIndex != 7)
            { //sound 7 is an empty sound used in some cases for this game
                StartCoroutine(InfoTextE());
            }
        }
    }

    IEnumerator InfoTextE()
    {
        FreezeControlsHandler.Instance.FreezeControlls();
        TextToSpeech.ins.playAudio(AccessibilityManager.instance.infolips[soundIndex]);
        CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.infolips[soundIndex].length);
        yield return new WaitForSeconds(AccessibilityManager.instance.infolips[soundIndex].length);
        FreezeControlsHandler.Instance.UnFreezeControlls();
        
        TextToSpeech.ins.playAudio(GetSound(list[currentListIndex]));
        CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(GetText(list[currentListIndex])), GetSound(list[currentListIndex]).length);
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
                        TextToSpeech.ins.playAudio(AccessibilityManager.instance.clips[soundIndex]);
                        CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.clips[soundIndex].length);
                        AccessibilityManager.instance.checkActivity = true;
                        delayToEnableAccessibility = AccessibilityManager.instance.clips[soundIndex].length;
                    }
                }
            }

            // Debug.Log("Index2 : " + soundIndex);
            Invoke("waitE", delayToEnableAccessibility);
        }
    }

    void waitE()
    {
        Debug.Log("TargetScript->waitE(" + gameObject.name + ")");
        FreezeControlsHandler.Instance.UnFreezeControlls();
        greenbox.SetActive(true);
        // GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
        EnableAccessability = true;
    }

    //public void move_with_click(GameObject obj)
    //{
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        if (obj.name == list[currentListIndex].name)
    //        {
    //            break;
    //        }
    //        moveForward();
    //    }
    //}

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
