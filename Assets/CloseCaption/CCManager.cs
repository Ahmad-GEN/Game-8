using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CloseCaption {

	public enum CCPriority {Lowest, Low, Medium, High, Highest} // Low for bg, High for dialogues
	public enum CCPlacement {Bottom, Top}

	public class CCManager : MonoBehaviour {

		public CCPlacement placementPos;
//		public List<Caption> captionsList;

		public Caption[] captionArray;

		public GameObject top, bottom;
		public Text topText, bottomText;

		public string[] seqStrings;
		public float[] seqTimes;

		public static CCManager instance;
		void Awake()
		{
			instance = this;
//			captionsList = new List<Caption> ();
			captionArray = new Caption[5];
		}

		public void LowCaption(string displayText)
		{
			CreateCaption (CCPriority.Medium, displayText, 1f);
		}

		public void CreateCaption (int seqStringIndex, float displayTime)
		{
			if (seqStringIndex < seqStrings.Length) {
				string captionText = seqStrings [seqStringIndex];
				float captionTime = displayTime;

				if (seqStringIndex < seqTimes.Length) {
					if (seqTimes [seqStringIndex] > 0f)
						captionTime = seqTimes [seqStringIndex];
				}

				if (captionText.Contains ("Order:"))
					CreateCaption (CCPriority.High, seqStrings [seqStringIndex], captionTime);
				else if (captionText.Contains ("["))
					CreateCaption (CCPriority.Low, seqStrings [seqStringIndex], captionTime);
				else
					CreateCaption (CCPriority.Medium, seqStrings [seqStringIndex], captionTime);
				
			} else {
				Debug.Log ("Index OutOfBound Exception: " + seqStringIndex);
			}
		}

		public void CreateCaption (CCPriority priority, int seqStringIndex, float displayTime)
		{
			if (seqStringIndex < seqStrings.Length)
				CreateCaption (priority, seqStrings [seqStringIndex], displayTime);
			else
				Debug.Log ("Index OutOfBound Exception: " + seqStringIndex);
		}

		public void CreateCaption (CCPriority priority, int seqStringIndex, float displayTime, CCPlacement placement)
		{
			if (seqStringIndex < seqStrings.Length)
				CreateCaption (seqStrings [seqStringIndex], displayTime, priority, placement);
			else
				Debug.Log ("Index OutOfBound Exception: " + seqStringIndex);
		}

		public void CreateCaption (string displayText, float displayTime, CCPriority priority = CCPriority.Medium, CCPlacement placement = CCPlacement.Bottom)
		{
			UpdatePlacementPosition (placement);
			CreateCaption (priority, displayText, displayTime);
		}

		public void CreateCaption (CCPriority priority, string displayText, float displayTime)
		{
			if(GameManager.Instance != null && !GameManager.Instance.Accessibilty)
				return;

			#if UNITY_EDITOR
			if (displayTime <= 0f)
				displayTime = 1f;
			#endif

			Caption caption = new Caption ();
			caption.priority = priority;
			caption.displayText = displayText;
			caption.endTime = Time.time + displayTime;
//			captionsList.Add (caption);

			captionArray [(int)caption.priority] = caption;

			Debug.Log ("Added: " + caption.priority.ToString () + ": " + caption.displayText + ", " + caption.endTime);
			UpdatePlacementPosition (placementPos);
			UpdateCaptions ();
		}

		public void UpdatePlacementPosition (CCPlacement newPlacementPos)
		{
			if(GameManager.Instance != null && !GameManager.Instance.Accessibilty)
				return;

			placementPos = newPlacementPos;
			top.SetActive (placementPos == CCPlacement.Top);
			bottom.SetActive (placementPos == CCPlacement.Bottom);
		}

		void Update()
		{
			if (Input.GetMouseButtonDown (0)) {
				UpdateCaptions ();
			}
		}

		void UpdateCaptions()
		{
			if(GameManager.Instance != null && !GameManager.Instance.Accessibilty)
				return;
				
//			Debug.Log ("UpdateCaptions");

//			for (int i = captionsList.Count - 1; i >= 0; i--) {
//				if ((captionsList [i].endTime <= Time.time) || (captionsList [i].displayText == "")) {
//					captionsList.Remove (captionsList [i]);
//				}
//			}
//			captionsList.Sort ((c1, c2) => c1.priority.CompareTo (c2.priority));

//			foreach (Caption caption in captionsList) {
//				Debug.Log (caption.priority.ToString () + ": " + caption.displayText + ", " + caption.endTime);
//			}

			for (int i = captionArray.Length - 1; i >= 0; i--) {
				if (captionArray [i].endTime > Time.time) {
					Invoke ("UpdateCaptions", captionArray [i].endTime - Time.time);
					topText.text = captionArray [i].displayText;
					bottomText.text = captionArray [i].displayText;
					return;
				}
			}

			top.SetActive (false);
			bottom.SetActive (false);
			/*
			// Update the last caption
			if (captionsList.Count > 0) {
				Invoke ("UpdateCaptions", captionsList [captionsList.Count - 1].endTime - Time.time);
				topText.text = captionsList [captionsList.Count - 1].displayText;
				bottomText.text = captionsList [captionsList.Count - 1].displayText;
			} else {
				top.SetActive (false);
				bottom.SetActive (false);
			}*/
		}
	}

	[System.Serializable]
	public struct Caption {
		public CCPriority priority;
		public string displayText;
		public float endTime;
	}
}