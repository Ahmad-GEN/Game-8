using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class EndPanel : MonoBehaviour {

	//public int AudioToPlay;
	//public GameObject Money;
	public GameObject FinalPanel02;
	GameManager Sound;
	Fade Mode;
	// Use this for initialization
	void Start () {
		Sound = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		Mode = gameObject.GetComponent<Fade> ();
		StartCoroutine (ShowMoney());
//		EventController.ins.Gametimer(false);
	//	EventController.ins.PrintReport();
		//StartCoroutine (wait ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public IEnumerator ShowMoney(){
		yield return new WaitForSeconds (7.5f);
		Mode.Fadeout = true;
		yield return new WaitForSeconds (1f);
		FinalPanel02.SetActive (true);
		this.gameObject.SetActive (false);

	}

}
