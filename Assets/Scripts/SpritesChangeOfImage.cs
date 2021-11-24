using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class SpritesChangeOfImage : MonoBehaviour {

	public Sprite[] ImageSprites;
	public GameObject ImageObject;
	public float DelayToChangeSprite;
	GameManager Manager;
	// Use this for initialization
	void Start () {
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		StartCoroutine (ChangeSprite ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator ChangeSprite(){

		for (int i = 0; i < ImageSprites.Length; i++) {
			ImageObject.GetComponent<Image> ().sprite = ImageSprites [i];
			ImageObject.SetActive (true);
			yield return new WaitForSeconds (DelayToChangeSprite);
			ImageObject.SetActive (false);
			ImageObject.GetComponent<CanvasGroup> ().alpha = 1;
		}
		yield return new WaitForSeconds (1.5f);
		//Debug.Log (Manager.CurrentLevelIndex);
		Manager.Panels_List [Manager.CurrentLevelIndex].SetActive (false);
		Manager.Panels_List [Manager.CurrentLevelIndex + 1].SetActive (true);
		Manager.CurrentLevelIndex++;

	}



}
