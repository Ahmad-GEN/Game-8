using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]

public class AccessibilityManager : MonoBehaviour
{
    #region Variables

    #region private
	public bool checkActivity = false;
    [HideInInspector]   public EventController EventControllerRef;
    public GameObject TextToSpeechObject;
    [HideInInspector]   public int total_object = 6;
    private int targetCounter = 0, destinationCounter = 0, LevelCounter = 0;
    private bool flag = true;
    public Direction mydirection, tempDirection; // contain current active state of game
    GameObject selectedTargetObject; // holds info of the obj which was selected when stae machine goes from target to destination state ..in order to give the info to game.
	public bool CanCheckInfo;
    #endregion
    #region public
    public int DelayToEnableFirstPanel = 35;
    public bool enablepause = false;
    public GameObject pausePanel;
    public bool DragnDrop = false;
    public bool isSingleDestination = false;
    public GameObject block;
    public GameObject GreenBox;
    public bool isCleanUP = false;
    [HideInInspector]
    public List<AudioClip> infolips = new List<AudioClip>();
//	[HideInInspector]
//	public bool close;
    [HideInInspector]
    public List<AudioClip> clips = new List<AudioClip>();
    [HideInInspector]
    public float timeOut = 30.0f, timeOutTimer = 0.0f; // countr to check for screen time out func
	[HideInInspector]
	public bool ShowPausePanel;
    public delegate void MultiDelegate(GameObject obj);
    public MultiDelegate DestinationSelected; // use to tell notify everyone on time of destination selection
    public bool gameWasPaused = false;
    public delegate void levelComplete(int nextlevel);
    public levelComplete levelCompleted; // use to tell notify everyone on time of level completion
    public static AccessibilityManager instance;
    public GameObject[] listHandleList; // handle list of all state machine in game
    public GameObject[] levelsList; // handle list of all destination which will change over time in game
    public GameObject[] targetList; // handle list of all target which will change over time in game
    public GameObject[] destiantionList; // handle list of all destination which will change over time in game
	[HideInInspector]
	public bool LastCheck;
    public enum Direction { MainMenu, GamePlay, Target, Destination, Ending, pause }; // states which will be enable through out the game
    public Dictionary<Direction, AcessibilityInterface> root = new Dictionary<Direction, AcessibilityInterface>(); // this dicionary is reponsible for storing all state and able to iterate it through game
    #endregion
	public GameObject Loading;
    #endregion
    #region  functions
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    //populating the root
    void Start()
    {
        //Loading.SetActive(true);
        //LastCheck = true;
        //CanCheckInfo = true;
        //ShowPausePanel = true;
        ////  print("aaccc");
        //if (block != null)
        //    block.SetActive(true);
        //if (GreenBox != null)
        //    GreenBox.SetActive(true);
        //if (TextToSpeechObject != null)
        //    TextToSpeechObject.SetActive(true);

        PlayerPrefs.SetString("clickable", "false");
        PlayerPrefs.SetInt("Click", 1);

        instance = this;
        if (listHandleList != null)
        {
            // TextToSpeech.ins.DowloadStartingSound(inGameSounds, clips);
            // TextToSpeech.ins.DowloadStartingSound(infoTexts, infolips);
            // for (int i = 0; i < 4; i++)
            // {
            //     clips.Add(AudioFile.instance.inGame[i]);
            //     infolips.Add(AudioFile.instance.infoText[i]);
            // }

            populateRootList();
			StartCoroutine (StartingAccessibilty ());
          //  Invoke("startAccessibilty", 4);
        }
    }

    private void OnEnable()
    {
        Loading.SetActive(true);
        LastCheck = true;
        CanCheckInfo = true;
        ShowPausePanel = true;
        //  print("aaccc");
        if (block != null)
            block.SetActive(true);
        if (GreenBox != null)
            GreenBox.SetActive(true);
        if (TextToSpeechObject != null)
            TextToSpeechObject.SetActive(true);
    }

    IEnumerator StartingAccessibilty()
	{
		yield return new WaitUntil(() => TextToSpeech.ins.AudioDownloaded==TextToSpeech.ins.TotalAudioToDownload);
		Loading.SetActive (false);
		startAccessibilty ();
	}
    public void startAccessibilty()
    {
        mydirection = Direction.MainMenu;
        root[mydirection].changeState(true);
      
    }
    ///<summary>
    /// this function is used to populate root from states which are gonna role in games
    ///</summary>
    public void populateRootList()
    {
        // listHandleList[2] = targetList[0];
        // listHandleList[3] = destiantionList[0];
        int i = 0;
        string[] allDirection = System.Enum.GetNames(typeof(Direction));
        if (allDirection.Length != listHandleList.Length)
        {
            Debug.Log(" the number of states and provided objects are not equal");
            return;
        }
//        AudioFile.instance.download();
        foreach (Direction currDirrection in System.Enum.GetValues(typeof(Direction)))
        {
            root.Add(currDirrection, listHandleList[i].GetComponent<AcessibilityInterface>());
            i++;
        }
    }
    /// <summary>
    /// Game Ending
    /// </summary>
    public void EndGame()
    {
        root[mydirection].changeState(false);
        mydirection = Direction.Ending;
        root[mydirection].changeState(true);
    }
    public Direction pauseGame()
    {
        Direction temp = mydirection;
        if (mydirection != Direction.MainMenu && mydirection != Direction.GamePlay && mydirection != Direction.Ending && enablepause)
        {
            print("pausing game");
            pausePanel.SetActive(true);
            root[mydirection].changeState(false);
            mydirection = Direction.pause;
            checkActivity = true;
            ShowPausePanel = false;
            root[mydirection].changeState(true);
        }
        return temp;

    }
    public void unpauseGame()
    {
        print("unpausing game");
        root[mydirection].changeState(false);
        mydirection = tempDirection;
        Debug.Log("Direction : " + mydirection);
        root[mydirection].changeState(true);
        if (mydirection == Direction.GamePlay)
        {
            Debug.Log("changing direction to target");
            root[mydirection].changeState(false);
            mydirection = Direction.Target;
            root[mydirection].changeState(true);
        }
        if (mydirection == Direction.Destination)
        {
            Debug.Log("changing direction to target");
            root[mydirection].changeState(false);
            mydirection = Direction.Destination;
            root[mydirection].changeState(true);
        }
        //ShowPausePanel = true;
    }
    /// <summary>
    /// this function is used when user want to changes the target e.g when going from one level to second.
    /// </summary>
    public void changeTarget()
    {
        targetCounter++; // in creasing target couter
        if (targetCounter < targetList.Length)
        {
            root[Direction.Target].changeState(false); // change state to false
            root[Direction.Target] = targetList[targetCounter].GetComponent<AcessibilityInterface>(); // replacing the target list.
            levelCompleted(targetCounter); // notified the games with level finish

            print("@@fdfdf");
            Invoke("changeGameplay", 4f);
        }
        else
        {
            EndGame();
            Debug.Log("End Game");
        }

    }
    public void changeGameplay()
    {
        while (!levelsList[LevelCounter])
        {
            print("waiting to active the level");
        }
        LevelCounter++;
        root[Direction.GamePlay] = levelsList[LevelCounter].GetComponent<AcessibilityInterface>();
        print("changing level " + levelsList[LevelCounter].name + "    " + targetCounter);

        root[mydirection].changeState(false);
        mydirection = Direction.GamePlay;// reversing the state to GamePlay.
        root[mydirection].changeState(true);
        StartCoroutine(Switch());
    }
    /// <summary>
    /// this function is used when user want to changes the destination e.g when going from one level to second.
    /// </summary>
    public void changeDestination()
    {
        destinationCounter++;
        if (destinationCounter < destiantionList.Length)
        {
            root[Direction.Destination] = destiantionList[destinationCounter].GetComponent<AcessibilityInterface>();
        }
    }
    /// <summary>
    /// this function is used to download in game sound for repetative use.
    /// </summary>
    void downloadInGameSounds()
    {

    }
    /// <summary>
    /// when user wants to shift forward from one state to another
    /// </summary>
    public void SwitchToNextState(GameObject obj)
    {
        if (obj != null)
            selectedTargetObject = obj;

        switch (mydirection)
        {
            case (Direction.MainMenu):

                root[mydirection].changeState(false);
                mydirection = Direction.GamePlay;
                root[mydirection].changeState(true);
                StartCoroutine(Switch());
                break;

            case (Direction.GamePlay):

                root[mydirection].changeState(false);
                mydirection = Direction.Target;
                root[mydirection].changeState(true);

                break;

            case (Direction.Target):

                root[mydirection].changeState(false);
                mydirection = Direction.Destination;
                root[mydirection].changeState(true);
                break;

            case (Direction.pause):

                root[mydirection].changeState(false);
                mydirection = tempDirection;
                root[mydirection].changeState(true);
                break;

            default:
                print("invalid state is selected");
                break;
        }


    }
    IEnumerator Switch()
    {
        // print("2222222222222222222222222222222222222222222222" + placee);
        if (placee == "first")
            yield return new WaitForSeconds(DelayToEnableFirstPanel);
        else
            yield return new WaitForSeconds(1f);

        root[mydirection].changeState(false);
        mydirection = Direction.Target;
        root[mydirection].changeState(true);
    }
    /// <summary>
    /// when user wants to shift backward from one state to another
    /// </summary>
    public void SwitchToPreviousState()
    {
        switch (mydirection)
        {
            case (Direction.Destination):

                root[mydirection].changeState(false);
                mydirection = Direction.Target;
                root[mydirection].changeState(true);

                break;

            case (Direction.Target):

                root[mydirection].changeState(false);
                mydirection = Direction.GamePlay;
                root[mydirection].changeState(true);

                break;
            default:
                print("invalid state is selected");
                break;
        }

    }
    public void resetCurrentState()
    {
        // root[mydirection].changeState(false);
        // root[mydirection].changeState(true);
    }
    // this function is used to check if user want to go back and re select the target.
    public void revertBackToTarget()
    {
        SwitchToPreviousState();
        root[mydirection].revertOption();
    }
    // this function is used when user select the target as a reponse we will send notification to all delegate subscribe to
    // DestinationSelected so they can perform any activity the games wants.
    public void AfterDestinationSelected(GameObject obj)
    {
        if (flag)
        {
            flag = false;
            DestinationSelected(obj);
            StartCoroutine(enable());
        }
    }

    IEnumerator enable()
    {
        yield return new WaitForSeconds(2f);
        flag = true;
    }
    #region userInput Fuction
    //<--------- User controls -------- called from index.html file ----------------->
    public void swipeUp()
    {
        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }
        // Mobile next button
        //if (!External.Instance.Preview)
        //{
        //    EventController.instance.CountScreenInteractionWithoutCheck();
        //}
        EventControllerRef.CountScreenInteractionWithoutCheck();
        root[mydirection].moveForward();
        checkActivity = true;
    }
    public void swipeDown()
    {
        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }
        // Mobile prev button
        //if(!External.Instance.Preview) {

        //	EventController.instance.CountScreenInteractionWithoutCheck ();
        //}
        EventControllerRef.CountScreenInteractionWithoutCheck();
        root[mydirection].moveBackward();
        checkActivity = true;
    }
    public void select()
    {
        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }
        //if(!External.Instance.Preview) 
        //      {	
        //	EventController.instance.CountScreenInteractionWithoutCheck ();
        //}
        EventControllerRef.CountScreenInteractionWithoutCheck();
        root[mydirection].select();
        checkActivity = true;
    }
    public void unSelect()
    {
        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }
        //  root[mydirection].unselect();
        //  checkActivity = true;
    }
    public void Close()
    {
        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }
        //if(!External.Instance.Preview) 
        //      {
        //	EventController.instance.CountScreenInteractionWithoutCheck ();
        //}
        EventControllerRef.CountScreenInteractionWithoutCheck();
        tempDirection = pauseGame();
    }

    public void Info()
    {
        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }
        EventControllerRef.CountScreenInteractionWithoutCheck();
        if (mydirection != Direction.Ending)
        {
			Debug.Log ("In Info");
            root[mydirection].infoText();
            checkActivity = true;
        }
    }
    #endregion
    //<-----------------------------------------------user controls------------------------------------------>
    void Update()
    {
        PlayerPrefs.SetString("clickable", "false");
        PlayerPrefs.SetInt("Click", 1);

        if (FreezeControlsHandler.Instance != null)
        {
            if (FreezeControlsHandler.Instance.isControllsFreezed)
            {
                return;
            }
        }

        ScreenTimeoutNotifier();

        bool down = Input.GetKeyDown(KeyCode.DownArrow);
        bool goBack = Input.GetKeyDown(KeyCode.Backspace);
        bool Up = Input.GetKeyDown(KeyCode.UpArrow);
        bool space = Input.GetKeyDown(KeyCode.Space);
        bool info = Input.GetKeyDown(KeyCode.I);
        bool close = Input.GetKeyDown(KeyCode.C);
        bool click = Input.GetKeyDown(KeyCode.Mouse0);

        if (down)
        {
            EventSystem.current.SetSelectedGameObject(null);
            swipeUp();
            checkActivity = true;
            down = false;
        }
        
        if (Up)
        {
            EventSystem.current.SetSelectedGameObject(null);
            swipeDown();
            checkActivity = true;
            Up = false;
        }

        if (space)
        {
            select();
            checkActivity = true;
            space = false;
        }

        if (goBack)
        {
            unSelect();
            checkActivity = true;
            goBack = false;
        }

		if (info && CanCheckInfo)
        {
            Info();
			//if(!External.Instance.Preview) {
			//	EventController.instance.CountScreenInteractionWithoutCheck ();
			//}
            checkActivity = true;
            info = false;
        }

		if (close && ShowPausePanel && LastCheck)
        {
//			ShowPausePanel = false;
            Close();
            // GameManager.Instance.Stop();
            //  ToggleNaviagtion(false);
            //  pausePanel.SetActive(true);

            close = false;
        }

        if (click)
        { EventSystem.current.SetSelectedGameObject(null); }

    }
    ///<summary>
    ///the function is used in order to give functionality of perform some action after some time like play a sound 
    ///</summary>

    public void ScreenTimeoutNotifier()
    {
        timeOutTimer += Time.deltaTime;
        // If screen is tapped, reset timer
        if (checkActivity)
        {
            timeOutTimer = 0.0f;
            checkActivity = false;
            //Dont active screensaver
        }
        // If timer reaches zero, start screensaver
		if (timeOutTimer > timeOut && CanCheckInfo && LastCheck)
        {
			
			Info();
			checkActivity = true;
			//info = false;
//            string text = root[mydirection].getWaitText();
//            TextToSpeech.ins.playAudioThroughText(text);
            //TextToSpeech.ins.playLongAudio(i,int.Parse(cookie.name)-1);

            timeOutTimer = -2.0f;

        }
    }

    IEnumerator main(List<AccessibiltyObject> accesabilityObject, List<GameObject> list)
    {
		yield return new WaitUntil(() => TextToSpeech.ins.AudioDownloaded==TextToSpeech.ins.TotalAudioToDownload);
        print("size is " + AudioFile.instance.MainMenu.Count);
        for (int i = 0; i < list.Count; i++)
        {
            AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.MainMenu[i], text = AudioFile.instance.MainMenuString[i] };
            accesabilityObject.Add(obj);
        }
    }
    public string placee = "";
    public void populateAccessibiltyList(List<AccessibiltyObject> accesabilityObject, List<GameObject> list, string place)
    {
        placee = place;
        accesabilityObject.Clear();

        if (mydirection == Direction.MainMenu)
        {
            StartCoroutine(main(accesabilityObject, list));
        }
        else if (place == "panelOne")
        {
            print("going in panelOne");
            for (int i = 0; i < list.Count; i++)
            {

                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.panelOne[i], text =  AudioFile.instance.panelOneString[i]};
                accesabilityObject.Add(obj);
            }
        }
        else if (place == "panelTwo")
        {
            print("going in panelTwo");
            for (int i = 0; i < list.Count; i++)
            {
                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.panelTwo[i], text =  AudioFile.instance.panelTwoString[i]};
                accesabilityObject.Add(obj);
            }
        }
        else if (place == "panelThree")
        {
            print("going in panelThree");
            for (int i = 0; i < list.Count; i++)
            {
                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.panelThree[i], text = AudioFile.instance.panelThreeString[i] };
                accesabilityObject.Add(obj);
            }
        }
        else if (place == "panelFour")
        {
            print("going in panelFour");
            for (int i = 0; i < list.Count; i++)
            {
                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.panelFour[i], text = AudioFile.instance.panelFourString[i] };
                accesabilityObject.Add(obj);
            }
        }
        else if (place == "ending" && mydirection != Direction.MainMenu && mydirection != Direction.GamePlay)
        {
            print("going in ending");
            for (int i = 0; i < list.Count; i++)
            {
                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.ending[i], text = AudioFile.instance.endingString[i] };
                accesabilityObject.Add(obj);
            }
        }
        else if (place == "destination")
        {
            print("going in destination");
            for (int i = 0; i < list.Count; i++)
            {
                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.destination[i], text = AudioFile.instance.destinationString[i] };
                accesabilityObject.Add(obj);
            }
        }
        else if (place == "pause")
        {
            print("going in pause");
            for (int i = 0; i < list.Count; i++)
            {
                AccessibiltyObject obj = new AccessibiltyObject() { gameobject = list[i], clip = AudioFile.instance.pause[i], text = AudioFile.instance.pauseString[i] };
                accesabilityObject.Add(obj);
            }
        }
    }
    public void ToggleNaviagtion(bool state)
    {
        root[mydirection].toggleNavigation(state);
    }
    #endregion

    public void End_Drag(GameObject obj)
    {
        if (!GameManager.Instance.Accessibilty)
            return;
        if (GameManager.Instance.gameNumber == 2)
        {
            if (!OnTriggerEnterForPackForTrip.ins.CanPutInside)
                return;
        }
        else
        {
            //Debug.Log("can put inside"+ OnTriggerEnterForOfficeMail.ins.CanPutInside);
            //if (!OnTriggerEnterForOfficeMail.ins.CanPutInside)
            //    return;
        }
        Debug.Log("End Drag function");
        targetList[targetCounter].GetComponent<TargetScript>().move_with_click(obj);
        Debug.Log("End Drag function1");
        select();
        Debug.Log("End Drag function2");
        checkActivity = true;
    }

    #region  UpdatedAccessibility
    public void mouse_click(GameObject obj)
    {
        if (GameManager.Instance.Accessibilty)
        {
            if (obj.name == "Play_Btn")
            {
                listHandleList[0].GetComponent<listhandler>().move_with_click(obj);
                select();
                checkActivity = true;
            }
            else if (obj.name == "PlayBtn")
            {
                //pausePanel.GetComponent<listhandler>().move_with_click(obj);
                if (FreezeControlsHandler.Instance.isControllsFreezed)
                    return;

                print("AccessibilityManager->select(Mydirection: " + mydirection + ")");
                EventController.instance.CountScreenInteractionWithoutCheck();
                Debug.Log("root[" + mydirection + "].select();");
                root[mydirection].select();
                checkActivity = true;
            }
            //else if (obj.name == "L1 Bag")
            //{
            //    list_mouse_click(0, obj);
            //}
            //else if (obj.name == "L2 Bag")
            //{
            //    list_mouse_click(1, obj);
            //}
            //else if (obj.name == "L3 Bag")
            //{
            //    list_mouse_click(2, obj);
            //}
            //else if (obj.name == "L4 Bag")
            //{
            //    list_mouse_click(3, obj);
            //}
            //else if (obj.name == "L5 Bag")
            //{
            //    list_mouse_click(4, obj);
            //}
        }
    }

    public void Target_mouse_click(GameObject obj)
    {
        Debug.Log("target_mouse_click");
        if (GameManager.Instance.Accessibilty)
        {
            targetList[targetCounter].GetComponent<TargetScript>().move_with_click(obj);

            //if (!SpacePressed)
            //    return;
            //select();
            print("AccessibilityManager->select(Mydirection: " + mydirection + ")");
            EventController.instance.CountScreenInteractionWithoutCheck();
            Debug.Log("root[" + mydirection + "].select();");
            root[mydirection].select();
            checkActivity = true;
        }
    }
    public void ResumeGame()
    {
        if (GameManager.Instance.Accessibilty)
        {
            unpauseGame();
        }
    }
    public void Paused()
    {
        //if (FreezeControlsHandler.Instance.isControllsFreezed)
        //    return;
        if (GameManager.Instance.Accessibilty && !GameManager.Instance.block.activeInHierarchy)/* && ShowPausePanel && LastCheck*/
        {
            //EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("PAUSE GAME");
            Close();
        }
    }
    #endregion
}

