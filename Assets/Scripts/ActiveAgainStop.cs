using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAgainStop : MonoBehaviour {

	// Use this for initialization
	public GameObject Again;
	public GameObject Stop;
	public float sec;
	void Start () {
		StartCoroutine (ActiveButtons ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ActiveButtons()
	{
		yield return new WaitForSeconds (sec);
		Again.SetActive (true);
		Stop.SetActive (true);
		
	}
}
