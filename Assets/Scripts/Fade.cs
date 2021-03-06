using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
	[HideInInspector]
	public bool FadeIn;
	[HideInInspector]
	public bool Fadeout;
	// Use this for initialization

	void OnEnable()
	{
		FadeIn = true;
	}
	void Update () {

		if (FadeIn) {
			this.gameObject.GetComponent<CanvasGroup> ().alpha += Time.deltaTime*0.85f;
			if (this.gameObject.GetComponent<CanvasGroup> ().alpha == 1) {
				FadeIn = false;
			}
		 }
		if (Fadeout) {
			this.gameObject.GetComponent<CanvasGroup> ().alpha -= Time.deltaTime*0.85f;
			if (this.gameObject.GetComponent<CanvasGroup> ().alpha == 0) {
				Fadeout = false;
			}
		}	
	}


}
