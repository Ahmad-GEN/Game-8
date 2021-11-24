using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartTask : MonoBehaviour {
	public GameObject PanelToOpen;
	public GameManager Manager;
	public Fade mode;
	bool CanFade=false;
	int Index;
	public float sec=5.5f;
	// Use this for initialization
	void Awake()
	{
		int.TryParse (this.gameObject.tag, out Index);
	}

	void OnEnable()
	{
		StartCoroutine (Wait ());
	}
	// Update is called once per frame
	void Update () {
		if (mode.Fadeout == false && CanFade) {
			for (int i = 0; i < Manager.TaskPanel.Length; i++) {
				Manager.TaskPanel [i].SetActive (false);
			}
			PanelToOpen.SetActive (true);
			CanFade = false;
		}
	}
	public IEnumerator Wait()
	{
		yield return new WaitForSeconds (sec);
		Manager.mode[Index].Fadeout = true;
		mode.Fadeout = true;
		CanFade = true;
	}

}
