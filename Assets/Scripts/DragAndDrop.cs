using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {
	[Space] public GameObject ObjectToDrag;//reference of the object which is going to be dragged 
	[HideInInspector]
	public Vector3 ObjectCentre; // gameobject centre which is to be dragged
	[HideInInspector]
	public Vector3 TouchPosition; //click or touch transform position
	[HideInInspector]
	public Vector3 Offset; // vector position between touchpoint/mouse click with object centre
	[HideInInspector]
	public Vector3 NewObjectCentre; //new centre of the object during dragging
	[HideInInspector]
	public bool CanTouchOrClick=false;
	GameManager Manager;
	[Space] public GameObject ObjectOriginalParent;
	[Space] public GameObject ObjectParentDuringDrag;
	RaycastHit hit; //stores hit information of the object
	[HideInInspector]
	public bool CanSetParent=false;
	[HideInInspector]
	public bool DraggingMode=false;
	float Width;
	float Height;
	public GameObject bottomLeftLimit;
	public GameObject topRightLimit;
	GameObject Selected;
	public float WidthPer=8f;
	public float HeightPer = 8f;

	public bool IsObjectInteractable = true;
	public float WaitToClickAfterStart=4.8f;

	OnTriggerEnterForPackForTrip TriggerForTrip;
	public string ColliderObjectName;
	public static DragAndDrop instance;

	// Use this for initialization
	void Start () {
		instance = this;
		if(GameManager.Instance.Accessibilty)
		CanTouchOrClick = false;
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		TriggerForTrip = GameObject.Find (ColliderObjectName).GetComponent<OnTriggerEnterForPackForTrip> ();
		StartCoroutine (WaitToClick (WaitToClickAfterStart));
		if (Screen.width == 1280 && Screen.height == 720) {
			Width = (Screen.width * 6.5f) / 1000;
			Height = (Screen.height * 6.5f) / 1000;
		} 
		else if(Screen.width == 850 && Screen.height == 450)
		{
			Width = (Screen.width * 9.6f) / 1000;
			Height = (Screen.height * 9.6f) / 1000;
			
		}
		else {
			Width = (Screen.width * WidthPer) / 1000;
			Height = (Screen.height * HeightPer) / 1000;
		}
	
	}

	void FixedUpdate () {
		
		//****************//
		//~Click to DRAG~//
		//***************//

		//if left mouse is clicked
		if (Input.GetMouseButtonDown (0)){// && CanTouchOrClick) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);//convert mouse click position into ray
			if (Physics.Raycast (ray, out hit)) { // if raycast is hit by a collider
				if (hit.collider.gameObject.tag == "RightTag" ||  hit.collider.gameObject.tag == "WrongTag" || 
					hit.collider.gameObject.tag == "c1" ||
					hit.collider.gameObject.tag == "c2" ||
					hit.collider.gameObject.tag == "c3") 
				{
					//Selected = hit.collider.transform.GetChild (0).gameObject;
					//Selected.SetActive (true);
					ObjectToDrag = hit.collider.gameObject;
					ObjectCentre = ObjectToDrag.transform.position;
					TouchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					Offset = TouchPosition - ObjectCentre;
					ObjectToDrag.transform.SetParent(ObjectParentDuringDrag.transform);
					DraggingMode = true;
				}
				if (hit.collider.gameObject.tag == "NotTagged") {
					TriggerForTrip.PlaySound (int.Parse (hit.collider.gameObject.name));
					StartCoroutine (TriggerForTrip.WaitToPlaySound (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.2f));
					CanTouchOrClick = false;
					StartCoroutine (WaitToClick (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.5f));
				}
			}
		}

		//if left mouse is held down
		if (Input.GetMouseButton (0)){ //&& CanTouchOrClick) {
			if (DraggingMode) {
				TouchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				NewObjectCentre = TouchPosition - Offset;
				NewObjectCentre.x = Mathf.Clamp(NewObjectCentre.x, -Width,Width);
				NewObjectCentre.y = Mathf.Clamp(NewObjectCentre.y, -Height, Height);
				ObjectToDrag.transform.position = new Vector3 (NewObjectCentre.x,NewObjectCentre.y, NewObjectCentre.z);
			}
		}

		//if left click is released
		if (Input.GetMouseButtonUp (0) && DraggingMode) {
			//Selected.SetActive (false);
			if (!CanSetParent) {
				ObjectToDrag.transform.SetParent(ObjectOriginalParent.transform);
				if (!TriggerForTrip.CanPutInside && ObjectToDrag.gameObject.tag != "NotTagged" && TriggerForTrip.CannotPlace) {
					//Debug.Log ("InDrag Drop");
					TriggerForTrip.PlaySound (int.Parse (ObjectToDrag.gameObject.name));
					StartCoroutine (TriggerForTrip.WaitToPlaySound (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.2f));
					CanTouchOrClick = false;
					StartCoroutine (WaitToClick (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.5f));
				}
//				else if (ObjectToDrag.gameObject.tag == "Untagged" && TriggerForTrip.CannotPlace) {
//					TriggerForTrip.PlaySound (int.Parse (ObjectToDrag.gameObject.name));
//				}
			}
			DraggingMode = false;
		}
	


		//****************//
		//~Touch to DRAG~//
		//***************//

		foreach(Touch touch in Input.touches)
		{
			switch (touch.phase) {
			//if finger touched
			case TouchPhase.Began:
				if (CanTouchOrClick) {
					Ray ray = Camera.main.ScreenPointToRay (touch.position);//convert finger touch position into ray
					if (Physics.SphereCast (ray, 0.3f, out hit)) { // if raycast is hit by a collider
						if (hit.collider.gameObject.tag == "RightTag" ||  hit.collider.gameObject.tag == "WrongTag" || 
							hit.collider.gameObject.tag == "c1" ||
							hit.collider.gameObject.tag == "c2" ||
							hit.collider.gameObject.tag == "c3") 
						{
						//	Selected = hit.collider.transform.GetChild (0).gameObject;
						//	Selected.SetActive (true);
							ObjectToDrag = hit.collider.gameObject;
							ObjectCentre = ObjectToDrag.transform.position;
							TouchPosition = Camera.main.ScreenToWorldPoint (touch.position);
							Offset = TouchPosition - ObjectCentre;
							ObjectToDrag.transform.SetParent(ObjectParentDuringDrag.transform);
							DraggingMode = true;

//							
						}
						if (hit.collider.gameObject.tag == "NotTagged") {
							TriggerForTrip.PlaySound (int.Parse (hit.collider.gameObject.name));
							StartCoroutine (TriggerForTrip.WaitToPlaySound (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.2f));
							CanTouchOrClick = false;
							StartCoroutine (WaitToClick (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.5f));
						}
					}
				}
				break;
				//if finger dragged after touch
			case TouchPhase.Moved:
				if (DraggingMode && CanTouchOrClick) {
					TouchPosition = Camera.main.ScreenToWorldPoint (touch.position);
					NewObjectCentre = TouchPosition - Offset;
					NewObjectCentre.x = Mathf.Clamp(NewObjectCentre.x, -Width,Width);
					NewObjectCentre.y = Mathf.Clamp(NewObjectCentre.y, -Height, Height);
					ObjectToDrag.transform.position = new Vector3 (NewObjectCentre.x, NewObjectCentre.y, NewObjectCentre.z);
				}
				break;
				//if finger removed after finising touch
			case TouchPhase.Ended:
				if (DraggingMode) {
				//	Selected.SetActive (false);
					if (!CanSetParent) {
						ObjectToDrag.transform.SetParent(ObjectOriginalParent.transform);
						if (!TriggerForTrip.CanPutInside && ObjectToDrag.gameObject.tag != "NotTagged" && TriggerForTrip.CannotPlace) {
							TriggerForTrip.PlaySound (int.Parse (ObjectToDrag.gameObject.name));
							StartCoroutine (TriggerForTrip.WaitToPlaySound (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.2f));
							CanTouchOrClick = false;
							StartCoroutine (WaitToClick (Manager.Sounds [int.Parse (ObjectToDrag.gameObject.name)].length + 0.5f));
						}

					}
					DraggingMode = false;
				}
				break;
			}

		}
	}

	public IEnumerator WaitToClick(float sec)
	{
		yield return new WaitForSeconds (sec);
		if(GameManager.Instance.Accessibilty == false)
		CanTouchOrClick = true;
	}
}
