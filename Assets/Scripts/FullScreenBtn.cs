using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenBtn : MonoBehaviour {
	// Use this for initialization
	public Sprite[] FullScreenIMG;
	public static FullScreenBtn Instance;
	[HideInInspector]
	public Image IMG;
	bool CanSetIMG=true;
	void Awake()
	{
		Instance = this;
		SetFullScreenButton();
	}
	void Start () {
		IMG = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (CanSetIMG) {
			if (!Screen.fullScreen) {
				IMG.sprite = FullScreenIMG [0];
			} else {
				IMG.sprite = FullScreenIMG [1];
			}
			CanSetIMG = false;
		}

		SetFullScreenButton();
	}

	void SetFullScreenButton()
	{
		if (PlayerPrefs.GetInt ("DisableFullScreen") == 1) {
			this.gameObject.SetActive (false);
		}
	}
}
