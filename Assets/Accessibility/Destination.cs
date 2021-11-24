using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destination : MonoBehaviour, AcessibilityInterface
{
    public int timeToWaitToPlayTextATStart = 3;
    public int soundIndex = 3;
    public string textToSayOnWait;
    public static Destination ins;
    public bool EnableAccessability = false;
    public List<GameObject> list;
    public List<AccessibiltyObject> accesabilityObject;
    public GameObject greenbox;
    RectTransform greenTransform;
    int currentListIndex = 0;
    bool enableTouch = false;
    bool isButton = false;
    GameObject buttonObject;
    public GameObject targetObject;

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
            ins = this;
            currentListIndex = 0;
            greenTransform = greenbox.GetComponent<RectTransform>();
            GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
        }


        // listTranform = list[currentListIndex];

        // StartCoroutine(CopyTransform(greenTransform, list[currentListIndex]));


    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void Awake()
    {
        if (GameManager.Instance.Accessibilty)
        {
            AccessibilityManager.instance.populateAccessibiltyList(accesabilityObject, list, "destination");
        }

    }
    void GreenBoxNaviagtionAndAudio(RectTransform green, GameObject gameObject)
    {
        if (gameObject.GetComponent<Button>()) // check if the object has button then in order to perform its event we will add a boolen
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
        green.parent = list.parent;
        //    green.SetSiblingIndex(0);
        green.anchorMin = list.anchorMin;
        green.anchorMax = list.anchorMax;
        green.anchoredPosition = list.anchoredPosition;
        green.sizeDelta = list.sizeDelta;
        green.eulerAngles = list.eulerAngles;
        enableTouch = true;



    }
    public void getObject(GameObject obj)
    {
        //		Debug.Log("setting object");
        targetObject = obj;
    }
    public void changeState(bool state)
    {
        currentListIndex = 0;
        Debug.Log("changing state of  " + gameObject.name + "    " + state);
        if (state)
        {
            GreenBoxNaviagtionAndAudio(greenTransform, list[0]);
            StartCoroutine(wait());
        }

        EnableAccessability = false;

    }
    void playsound(GameObject gameObject)
    {
        AccessibiltyObject tempAccessibiltyObject = accesabilityObject.Find(y => y.gameobject == gameObject);
        
        if (tempAccessibiltyObject != null)// will call to play sound by find object in accessability 
        {
            // TextToSpeech.ins.playAudio(accesabilityObject.Find(y => y.gameobject == gameObject).clip);
			Debug.Log ("Playing Sound");
            
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
                currentListIndex--;
                playsound(list[currentListIndex]);
                GreenBoxNaviagtionAndAudio(greenTransform, list[currentListIndex]);
            }
            else
            {
                currentListIndex = list.Count - 1;
                playsound(list[currentListIndex]);
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

                //StartCoroutine(CopyTransform(greenTransform, list[currentListIndex + 1]));
                //                Debug.Log("down " + currentListIndex);
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
            if (AccessibilityManager.instance.DragnDrop)
            {
               // greenbox.transform.parent = targetObject.transform;
                AccessibilityManager.instance.AfterDestinationSelected(list[currentListIndex]);
            }
            else
            {
                print("simple");
                if (isButton)
                {
                    Debug.Log("touch of " + gameObject.name);
                    isButton = false;
                    buttonObject.GetComponent<Button>().onClick.Invoke();
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
		if (TextToSpeech.ins != null) {
			TextToSpeech.ins.playAudio (AccessibilityManager.instance.infolips [soundIndex]);
            CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.infolips [soundIndex].length);
			Debug.Log ("Index : " + soundIndex);

		}
    }
    public string getWaitText()
    {
        return textToSayOnWait;
    }
    public void revertOption()
    {

    }
    public void toggleNavigation(bool state)
    {
        EnableAccessability = state;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(timeToWaitToPlayTextATStart);
        // if (AccessibilityManager.instance.clips.Count > 0)
        //     TextToSpeech.ins.playAudio(AccessibilityManager.instance.clips[soundIndex]);
		if (TextToSpeech.ins != null) {
			TextToSpeech.ins.playAudio (AccessibilityManager.instance.clips [soundIndex]);
            CloseCaption.CCManager.instance.CreateCaption(CloseCaption.StringMerger.MergeStrings(AudioFile.instance.inGameString[soundIndex]), AccessibilityManager.instance.clips [soundIndex].length);
			Debug.Log ("Index2 : " + soundIndex);
			EnableAccessability = true;
		}
    }
}
