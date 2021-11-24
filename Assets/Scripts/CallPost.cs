using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPost : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		if (EventController.instance != null && !External.Instance.Preview) {
			EventController.instance.currentGamePercentage ();
		}
	}
}
