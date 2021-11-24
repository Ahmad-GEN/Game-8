using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Wait ());
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (Wait ());
	}
	IEnumerator Wait()
	{
		yield return new WaitForSeconds (1f);
		this.gameObject.SetActive (false);
	}
}
