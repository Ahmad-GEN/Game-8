using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Info : MonoBehaviour {

	public Animator InfoBtn;
	bool CanPlayInfoSound=false;
	public GameManager Manager;
	public DragAndDrop Ref;
	bool LevelComp=false;
	public float WaitToPlayAtStart;
	public float TransitionDelay=3f;
	// Use this for initialization
	void Start () {
		StartCoroutine (WaitToPlaySound (WaitToPlayAtStart));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenInfo(int ind)
	{
		if (!LevelComp) {
			if (CanPlayInfoSound) {
				
					Manager.Audio.clip = Manager.Sounds [ind];
					Debug.Log("GameManager->FadeOnClickBtn(" + ind + ")");
					CloseCaption.CCManager.instance.CreateCaption(ind, Manager.Audio.clip.length);
					Manager.Audio.Play ();
				
				CanPlayInfoSound = false;
				StartCoroutine (WaitToPlaySound (TransitionDelay));
				Ref.CanTouchOrClick = false;
				StartCoroutine (Ref.WaitToClick (2.8f));
			}
			InfoBtn.gameObject.SetActive (true);
			InfoBtn.SetBool ("CloseInfo", false);
			InfoBtn.SetBool ("OpenInfo", true);
		}

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
