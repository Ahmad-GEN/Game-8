using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloseCaption {
	public class StringMerger : MonoBehaviour {

		public static string MergeStrings (string a)
		{
			string formatedTitle = FormatIfTitle(a);

			return formatedTitle == null ? MergeStrings (new string[] { a }) : formatedTitle;
		}

		public static string MergeStrings (string a, string b)
		{
			return MergeStrings (new string[] { a, b });
		}

		public static string MergeStrings (string a, string b, string c)
		{
//			Debug.Log (a);
//			Debug.Log (b);
//			Debug.Log (c);
			return MergeStrings (new string[] { a, b, c });
		}

		
		public static string MergeStrings (string[] allStrings)
		{
			if (allStrings.Length == 0)
				return "";

			string finalString = "";

			if (allStrings.Length == 1)
				finalString = allStrings [0];
			else {
				finalString = allStrings [0];

				for (int i = 1; i < allStrings.Length; i++) {
					finalString += " " + allStrings[i];
				}
			}

			finalString = finalString.ToLower ();
//			Debug.Log (finalString);
			finalString  = finalString.Replace ("kindly ", "");

			while ((finalString.Length >= 2) && (finalString.Substring (0, 1) == " ")) {
				finalString = finalString.Substring (1, finalString.Length - 1);
			}

			finalString = finalString.Replace ("  ", " ");
			finalString = finalString.Replace ("  ", " ");
			finalString = finalString.Replace (".", "");

			// char[] allCharacters = finalString.ToCharArray ();
			
			if(CompareStrings(finalString, "press play to start", "navigate through personal things", "navigate through clothes"
							, "navigate through fun things", "navigate through personal clothes", "navigate through casual clothes"
							, "pause", "play", "play again", "end game","continue"))
            	finalString = char.ToUpper(finalString[0]) + finalString.Substring (1, finalString.Length - 1);
			else if(CompareStrings(finalString, "t shirt"))
				finalString = "T-shirt";
			else if(CompareStrings(finalString, "flip flops"))
				finalString = "flip-flops";
			
			// finalString = new string (allCharacters);

//			Debug.Log (finalString);
			return finalString;
		}

        static string FormatIfTitle(string text)
        {
			if(text.Equals("PACK FOR A BEACH TRIP"))
				return "Pack for a beach trip";
			else if(text.Equals("PACK FOR A TRIP TO A BIRTHDAY PARTY"))
				return "Pack for a trip to a birthday party";
            else if (text.Equals("PACK FOR A CAMPING TRIP"))
                return "Pack for a camping trip";
            else if (text.Equals("PACK FOR A SKIING TRIP"))
                return "Pack for a skiing trip";
            else if (text.Equals("PACK FOR A TRIP TO THE CAPITAL CITY"))
                return "Pack for a trip to the capital city";

			return null;
        }

		static bool CompareStrings(string strToCompare, params string[] strsToCompareWith)
		{
			for(int i = 0; i < strsToCompareWith.Length; i++)
			{
				if(strToCompare.Equals(strsToCompareWith[i]))
					return true;
			}

			return false;
		}
	}
}