using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class OnTriggerenter : MonoBehaviour {

	public string ObjectTag;
	public GameObject Collector;
	public DragAndDrop Ref;
	public GameObject Next_Btn;
	public bool CanPutInside = false;
	GameObject Temp;
	public GameObject CoverLid;
	public GameObject[] ObjectsToAppear;
	GameManager Manager;
	public Animator InfoBtn;
	bool CanPlayInfoSound=false;
	public DragAndDrop mode;
	public int LevelIndexNew;
	public static OnTriggerenter ins;
	// Use this for initialization
	void Start () {
		ins = this;
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		StartCoroutine (WaitToPlaySound (3.8f));
	}

	//void OnEnable()
	//{
	//	if (GameManager.Instance.Accessibilty)
	//	{
	//		AccessibilityManager.instance.DestinationSelected += check;
	//		Invoke("DelaYToCall", 4f);
	//		//  AccessibilityManager.instance.levelCompleted += LevleCompletedAccessibility;
	//	}
	//}

	public void PlaySound(int ind)  //play sound function with both unity audio source and html audio play
	{
		  Manager.Audio.clip = Manager.Sounds [ind];
		  CloseCaption.CCManager.instance.CreateCaption(ind, Manager.Audio.clip.length);
		   Manager.Audio.Play ();
		
	}

	public void OnMouseBtnUp(int ObjectInd)   //perform functionality when object is in boundaries of container and mouse clicking or touch is finised 
	{
		Temp.gameObject.SetActive (false);
		ObjectsToAppear[ObjectInd].SetActive (true);
		PlaySound(6);
		PlayerPrefs.SetInt ("AmountCollected", PlayerPrefs.GetInt ("AmountCollected") + 1);
	}
	

	void Update () {

		if (PlayerPrefs.GetInt("AmountCollected")==5) {
			//Next_Btn.SetActive (true);
			Manager.FadeOnClickBtn (LevelIndexNew);
			InfoBtn.gameObject.transform.GetComponentInParent<Button>().enabled = false;
		}
			
		if ((Input.GetMouseButtonUp (0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && CanPutInside) {
			if (Temp.gameObject.name == "01") {
				OnMouseBtnUp (0);
			} else if (Temp.gameObject.name == "02") {
				OnMouseBtnUp (1);
			}else if (Temp.gameObject.name == "03") {
				OnMouseBtnUp (2);
			}else if (Temp.gameObject.name == "04") {
				OnMouseBtnUp (3);
			}else if (Temp.gameObject.name == "05") {
				OnMouseBtnUp (4);
			}
			Ref.CanSetParent = true;
			Ref.DraggingMode = false;
			CanPutInside = false;
			StartCoroutine (wait ());
		}
	}

	void OnTriggerEnter(Collider Col) //on object enter inside the container
	{
		if (Col.gameObject.tag == ObjectTag && Ref.DraggingMode) {
			Temp = Col.gameObject;
			CanPutInside = true;
		}
	}

	void OnTriggerExit(Collider Col) //on object exit from the container
	{
		if (Col.gameObject.tag == ObjectTag) {
			Ref.CanSetParent = false;
			CanPutInside = false;
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
		for (int i = 0; i < ObjectsToAppear.Length; i++) {
			ObjectsToAppear [i].SetActive (false);
		}
		CoverLid.SetActive (true);
		Next_Btn.SetActive (false);
	}

	//for opening and closing info button
	public void OpenInfo(int ind)
	{
		if (CanPlayInfoSound) {
			
				Manager.Audio.clip = Manager.Sounds[ind];
				CloseCaption.CCManager.instance.CreateCaption(ind, Manager.Audio.clip.length);
				Manager.Audio.Play ();
			
			CanPlayInfoSound = false;
			StartCoroutine (WaitToPlaySound (3f));
			mode.CanTouchOrClick = false;
			StartCoroutine (mode.WaitToClick (2.8f));
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
}
