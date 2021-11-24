using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class OnTriggerForAuntsBirthday : MonoBehaviour {
	
	public string ObjectTag;
	public GameObject Collector;
	public DragAndDrop Ref;
	public GameObject Next_Btn;
	[HideInInspector]
	public bool CanPutInside=false;
	GameObject Temp;
	//public GameObject CoverLid;
	public GameObject[] ObjectsToAppearInCollider;
	public int[] ObjectsToAppearInCollider_Index;
	//ActiveNewObjectsForPetStore NewObjectsInstance;
	GameManager Manager;
	public Animator InfoBtn;
	bool CanPlayInfoSound=false;
	public DragAndDrop mode;


	bool NewEnvelopes = false;
	//public int LevelIndex;
	//public GameObject[] AlreadyPlaced;
	bool IsLevelEnd = false;

	//By Aleem

	[HideInInspector]
	public bool CannotPlace=true;
	public GameObject LevelEndAnimation;

	[HideInInspector]
	public int ObjectsAppearInCollider_Index = 0;
	public int AmountCollectedForLevelEnd;

	public int LevelEndVoiceIndex;

	public GameObject []Tags;
	public Transform[] ReferencePositions;
	//public int[] ObjectNameSounds;

	public float WaitToPlayAndClick;
	public float PlaySoundInfo = 3.2f;

	public float WaitToPlayRightSound;  //Delay To Play Right Sound After Placing a Correct Item

	public float WaitToPlayOtherObjectSound;  //Delay To Play Other Object's Sound After Placing a Correct Item
	public int[] WrongSounds;
	//public bool IsObjectInteractable = true;
	public GameObject InvisiblePanel;

	RaycastHit hit;


	// Use this for initialization
	void Start () {
		//NewObjectsInstance = transform.GetComponentInParent<ActiveNewObjectsForPetStore> ();
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		StartCoroutine (WaitToPlaySound (PlaySoundInfo));


	}
	public void PlaySound(int ind)  //play sound function with both unity audio source and html audio play
	{
		  
			Manager.Audio.clip = Manager.Sounds [ind];
			Debug.Log("GameManager->FadeOnClickBtn(" + ind + ")");
			CloseCaption.CCManager.instance.CreateCaption(ind, Manager.Audio.clip.length);
			Manager.Audio.Play ();
		


	}




	//By Aleem
	public IEnumerator LevleComplete()
	{
		//Debug.Log ("Good Job.. Its Level End");
		//Debug.Log (Manager.CurrentLevelIndex);

		//StartCoroutine (mode.WaitToClick (WaitToPlayOtherObjectSound));
		InvisiblePanel.SetActive(true);
		yield return new WaitForSeconds (2.0f);
		LevelEndAnimation.SetActive (true);
		//		yield return new WaitForSeconds (1.5f);
		PlaySound (LevelEndVoiceIndex);

		yield return new WaitForSeconds (1.5f);
		PlaySound (LevelEndVoiceIndex + 1);
		yield return new WaitForSeconds (2.5f);
		Fade mod = Manager.Panels_List [Manager.CurrentLevelIndex].GetComponent<Fade> ();

		yield return new WaitForSeconds (1.0f);
		mod.Fadeout = true;
		yield return new WaitForSeconds (1.5f);
		InvisiblePanel.SetActive (false);
		Manager.Panels_List [Manager.CurrentLevelIndex].SetActive (false);
		Manager.Panels_List [Manager.CurrentLevelIndex+1].SetActive (true);
		Manager.CurrentLevelIndex++;


	}


	void Update () {

		if (AmountCollectedForLevelEnd == PlayerPrefs.GetInt ("AmountCollected")) {
			mode.CanTouchOrClick = false;
			StartCoroutine (mode.WaitToClick (4.0f));
			StartCoroutine (LevleComplete ());
			PlayerPrefs.SetInt ("AmountCollected", 0);
		}

		if ((Input.GetMouseButtonUp (0) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)) && CanPutInside) {


			Temp.gameObject.SetActive (false);
			CanPlayInfoSound = false;
			StartCoroutine (WaitToPlaySound (WaitToPlayOtherObjectSound));
			mode.CanTouchOrClick = false;
			StartCoroutine (mode.WaitToClick (WaitToPlayOtherObjectSound));

			//			if (ObjectsAppearInCollider_Index > 0 && (Manager.CurrentLevelIndex == 2 || Manager.CurrentLevelIndex == 4 || Manager.CurrentLevelIndex == 6)) {
			//ObjectsToAppearInCollider [int.Parse(mode.ObjectToDrag.name)].SetActive(true);
			//			}
			PlayerPrefs.SetInt ("AmountCollected", PlayerPrefs.GetInt ("AmountCollected") + 1);
			//Debug.Log (PlayerPrefs.GetInt ("AmountCollected"));
			for (int i = 0; i < ObjectsToAppearInCollider_Index.Length; i++) {
				if (int.Parse(mode.ObjectToDrag.name) == ObjectsToAppearInCollider_Index [i]) {
					ObjectsToAppearInCollider [i].SetActive (true);
				}
			}

			//ObjectsToAppearInCollider [ObjectsAppearInCollider_Index].SetActive(true);
			//AlreadyPlaced.SetActive (false);
			ObjectsAppearInCollider_Index++;
			//ObjectsToAppear.SetActive (true);
			//Debug.Log("True sound");
			PlaySound (int.Parse (mode.ObjectToDrag.name));
			StartCoroutine (WaitToPlayTrueSound (WaitToPlayRightSound));

			//			NewObjectsInstance.ObjectPresentInScene--;
			//			if (NewObjectsInstance.ObjectPresentInScene == 0 && PlayerPrefs.GetInt("AmountCollected")<AmountCollectedForLevelEnd ) {
			//				NewObjectsInstance.ActiveNewObjects (PlayerPrefs.GetInt ("NoOfObjectsIndex"));
			//				PlayerPrefs.SetInt ("NoOfObjectsIndex", PlayerPrefs.GetInt ("NoOfObjectsIndex") + 1);
			//			}
			//			if (PlayerPrefs.GetInt("AmountCollected")==AmountCollectedForLevelEnd) {
			//				//Debug.Log ("Level Complete");
			//				PlayerPrefs.SetInt ("AmountCollected", 0);
			//				StartCoroutine(LevleComplete());
			//				PlayerPrefs.SetInt ("NoOfObjectsIndex", 1);
			//				IsLevelEnd = true;
			//			}




			Ref.CanSetParent = true;
			Ref.DraggingMode = false;
			CanPutInside = false;
			StartCoroutine (wait ());
		}

		//		if (Manager.NewObjectsCheck) {
		//			ActiveNewObjects (PlayerPrefs.GetInt("NoOfObjectsIndex"));
		//			PlayerPrefs.SetInt ("NoOfObjectsIndex", PlayerPrefs.GetInt ("NoOfObjectsIndex") + 1);
		//			Debug.Log (PlayerPrefs.GetInt ("NoOfObjectsIndex"));
		//			Manager.NewObjectsCheck = false;
		//		}



		else if ((Input.GetMouseButtonUp (0) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)) && !CannotPlace) {

			//Debug.Log ("In Wrong");
			mode.ObjectToDrag.gameObject.tag = "Untagged";
			PlaySound (WrongSounds[int.Parse(mode.ObjectToDrag.name)]);
			//mode.CanTouchOrClick = false;
			//StartCoroutine (mode.WaitToClick (Manager.Sounds[WrongSounds[int.Parse(mode.ObjectToDrag.name)]].length));

			//To Wait for Sound and Touch While Wrong Sounds are Playing.
			CanPlayInfoSound = false;
			StartCoroutine (WaitToPlaySound (Manager.Sounds[WrongSounds[int.Parse(mode.ObjectToDrag.name)]].length));
			mode.CanTouchOrClick = false;
			StartCoroutine (mode.WaitToClick (Manager.Sounds[WrongSounds[int.Parse(mode.ObjectToDrag.name)]].length));

			//To make the wrong Object Disable.
			mode.ObjectToDrag.GetComponent<BoxCollider> ().enabled = false;
			Image Im = mode.ObjectToDrag.gameObject.GetComponent<Image>();
			Color co = Im.color;

			co.b = 255f;
			co.g = 255f;
			co.r = 255f;
			co.a = 100f;
			Im.color = co;
			mode.IsObjectInteractable = false;
			//mode.ObjectToDrag.gameObject.tag = "Untagged";

			for (int i = 0; i < Tags.Length; i++) {
				Tags[i].gameObject.transform.position = ReferencePositions [i].transform.position;
			}
			//			NewObjectsInstance.Tags[0].gameObject.transform.position = NewObjectsInstance.ReferencePositions [0].transform.position;
			//			NewObjectsInstance.Tags[1].gameObject.transform.position = NewObjectsInstance.ReferencePositions [1].transform.position;
			//			NewObjectsInstance.Tags[2].gameObject.transform.position = NewObjectsInstance.ReferencePositions [2].transform.position;

			StartCoroutine (wait ());
			CannotPlace = true;
		}

		else if ((Input.GetMouseButtonUp (0) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended))) {


			for (int i = 0; i < Tags.Length; i++) {
				Tags[i].gameObject.transform.position = ReferencePositions [i].transform.position;

			}

			//			CanPlayInfoSound = false;
			//			StartCoroutine (WaitToPlaySound (0.5f));
			//			mode.CanTouchOrClick = false;
			//			StartCoroutine (mode.WaitToClick (0.5f));
			//			Debug.Log("Tag" + mode.ObjectToDrag.tag);
			//Debug.Log("Name" + mode.ObjectToDrag.name);
			//				if((mode.ObjectToDrag.tag == "c1"  || mode.ObjectToDrag.tag == "c3") && mode.CanTouchOrClick){
			//					//Debug.Log (hit.collider.gameObject.name);
			//					PlaySound (int.Parse (mode.ObjectToDrag.gameObject.name));
			//					//PlaySound (19);
			//				}

			//Debug.Log (mode.ObjectToDrag.name);

		}
	}

	void OnTriggerEnter(Collider Col) //on object enter inside the container
	{

		if (Col.gameObject.tag == ObjectTag) {
			Temp = Col.gameObject;

			CanPutInside = true;
			NewEnvelopes = true;

		}
		if (Col.gameObject.tag != ObjectTag) {
			Temp = Col.gameObject;
			CannotPlace = false;
		}
	}

	void OnTriggerExit(Collider Col) //on object exit from the container
	{
		if (Col.gameObject.tag == ObjectTag) {
			Ref.CanSetParent = false;
			CanPutInside = false;
			NewEnvelopes = false;
			if (Input.GetMouseButtonUp (0)) {
				PlaySound (5);
			}
		} 

		if (Col.gameObject.tag != ObjectTag) {
			Temp = Col.gameObject;
			CannotPlace = true;
		}
	}

	IEnumerator wait()
	{
		yield return new WaitForSeconds (0.3f);
		Ref.CanSetParent = false;

	}

	public void LevelEnd() //called at end of each level
	{
		PlayerPrefs.SetInt ("AmountCollected", 0);
		//ObjectsToAppear.SetActive (false);
		Next_Btn.SetActive (false);
	}

	//for opening and closing info button
	public void OpenInfo(int ind)
	{
		if (CanPlayInfoSound) {
			
				Manager.Audio.clip = Manager.Sounds[ind];
				Debug.Log("GameManager->FadeOnClickBtn(" + ind + ")");
				CloseCaption.CCManager.instance.CreateCaption(ind, Manager.Audio.clip.length);
				Manager.Audio.Play ();

			
			CanPlayInfoSound = false;
			StartCoroutine (WaitToPlaySound (WaitToPlayAndClick));
			mode.CanTouchOrClick = false;
			StartCoroutine (mode.WaitToClick (WaitToPlayAndClick));

		}
		InfoBtn.gameObject.SetActive (true);
		InfoBtn.SetBool ("CloseInfo", false);
		InfoBtn.SetBool ("OpenInfo", true);
	}

	public void CloseInfo()
	{

		InfoBtn.SetBool ("CloseInfo", true);
		InfoBtn.SetBool ("OpenInfo", false);
		StartCoroutine (WaitSec ());


	}

	IEnumerator WaitToPlaySound(float sec)
	{
		yield return new WaitForSeconds (sec);
		CanPlayInfoSound = true;

	}

	IEnumerator WaitSec()
	{
		yield return new WaitForSeconds (0.5f);
		InfoBtn.gameObject.SetActive (false);

	}

	IEnumerator WaitToPlayTrueSound(float nowPlay)
	{
		yield return new WaitForSeconds (nowPlay);
		//Debug.Log ("Thing Name");
		PlaySound (28);

	}
}
