using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class WallPostAnimation : MonoBehaviour {
	public Animator[] Anim;
	public float WaitToAnimate;
	public GameObject OpenBag;
	public GameManager Manager;
	[DllImport("__Internal")]
	private static extern void NewScene(int ind);
	public Fade mode;
	// Use this for initialization
	void Start () {
		Debug.Log (Manager.CurrentLevelIndex);
		StartCoroutine (Animate (WaitToAnimate));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public IEnumerator Animate(float Sec)
	{
		yield return new WaitForSeconds (Sec);
		for (int i = 0; i < Anim.Length; i++) {
			Anim [i].SetBool ("CanAnim", true);
			yield return new WaitForSeconds (0.5f);
		}
		//Debug.Log (Manager.CurrentLevelIndex);
		if (Manager.CurrentLevelIndex == 1) {
			OpenBag.SetActive (true);
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (StartNextLevel ());
		}
	}

	public IEnumerator StartNextLevel()
	{
		mode.Fadeout = true;
		yield return new WaitForSeconds (1.5f);

		Manager.CurrentLevelIndex++;
//		Debug.Log ("Next Screen");
		StartCoroutine (Manager.NextLevel (2));
	}


	public void LoadGameOnPicture(int gameIndex)
	{
		Time.timeScale = 1f;
		NewScene (gameIndex);
		//SceneManager.LoadScene (gameIndex, LoadSceneMode.Single);
	}
}
