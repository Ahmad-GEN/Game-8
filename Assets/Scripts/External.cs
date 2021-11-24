using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Newtonsoft;
using Newtonsoft.Json;
using System;
public class External : MonoBehaviour
{
    public static External Instance;
    //public DemoDataEnter Ref;					   

    public bool Preview;
    public bool isAccessibilityLocalTest;
    public GameManager Manager;
    [HideInInspector]
    private string baseUrl;
    public TextToSpeech TTSRef;
    bool IsAccessibility;
    [HideInInspector]
    public string KeyNew;
    [HideInInspector]
    public GameStudentModel Model;

    public string BaseUrl
    {
        get
        {
            return baseUrl;
        }

        set
        {
            baseUrl = value;
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        //KeyNew = "0a6f6b69f5274fe0a2e23f22ab98b502";
												

        Instance = this;
        //GetGameAndStudentForScoring("{"+"\"GameId\":112"+","+"\"StudentId\":null"+","+"\"IsPreview\":true"+"}");
#if UNITY_EDITOR //for testing purposes in editor
        if (isAccessibilityLocalTest)
        {
            EnableAccessibilty("true");
            SetKey("c406eafe82dd4262a6596ebf745f7aa7");
        }
#else
		// SetAccessibilityByURL();
#endif
    }

    public void DisableFullScreen()
    {
        PlayerPrefs.SetInt("DisableFullScreen", 1);
    }

    public void PlayUnityScene()
    {
        Manager.StartCoroutine(Manager.LoadScene());
    }

    public void GetUrlFromServer(string URL)
    {
        Debug.Log("The Url from server is : " + URL);
        //		Ref.PostData (URL);
    }

    public void GetBaseUrl(string uRL)
    {
        //api/GameAssetApi/GetGameAssets?gameId=PlayerScoreApi
        //	Debug.Log ("The Base Url from server is : " + BaseURL+"n2y.Web.Api.L3Skills/ApiControllers/L3Skills/Player/PlayerScoreApiController/SavePlayerScore?gameId=107");
        Debug.Log("The Base Url from server is : " + uRL);
        BaseUrl = uRL;
    }
    public void GetGameAndStudentForScoring(string Json)
    {
        //Debug.Log ("Json Received");
        Debug.Log("The Json from server is : " + Json);
        if (Json.Contains("\"StudentId\":null"))
        {
            string output = Json.Replace("\"StudentId\":null", "\"StudentId\":0");
            Debug.Log("OutPut changed because student ID is Null : " + output);
            Model = JsonConvert.DeserializeObject<GameStudentModel>(output);
        }
        else
        {
            Model = JsonConvert.DeserializeObject<GameStudentModel>(Json);
        }

        // ModelData.SetGameId(Model.GameId);
        // ModelData.SetStudentId(Model.StudentId);
        // ModelData.SetAccessibiltyBool(Model.IsPreview);
		
        Preview = Model.GetAccessibiltyBool();
		  
        Debug.Log("Game ID : " + Model.GetGameId());
        Debug.Log("Student ID : " + Model.GetStudentId());
        Debug.Log("IsPreview : " + Model.GetAccessibiltyBool());
        // Debug.Log("EnableAccessibility : " + ModelData.GetAccessibilty());

        // StartCoroutine(WaitToPerformFunctionality(Model.EnableAccessibility));
																		  
																	   
																	
		  
																  
   

    }
    public void EnableAccessibilty (string newValue)
	{
		Debug.Log ("EnableAccessibilty (" + newValue + ")");
		StartCoroutine (WaitToPerformFunctionality (newValue == "true"));
	}
															  
													

    // public void GetAccessibilty(string state)
    // {
    //     StartCoroutine(WaitToPerformFunctionality(state == "true"));
    // }

    IEnumerator WaitToPerformFunctionality(bool ISAccessibilty)
    {
        yield return new WaitForSeconds(0.15f);
        if (ISAccessibilty == true)
        {
            Manager.Accessibilty = true;
            Manager.AccessibiltyObject.SetActive(true);
            //Debug.Log (BaseUrl + "api/PlayerScoreApi/SavePlayerScore");
        }
        else
        {
            Manager.Accessibilty = false;
            Manager.AccessibiltyObject.SetActive(false);
        }
    }

    IEnumerator WaitToPost()
    {
        yield return new WaitForSeconds(2f);
							   
								 
											  
        //		Ref.PostData (BaseUrl+"api/PlayerScoreApi/SavePlayerScore");
    }
    // public void EnableAccessibilty(string state) //for local functionality
    // {
    //     IsAccessibility = state == "true";
    // }

    public void SetKey(string NewKey)//for local functionality
    {
        if (IsAccessibility)
        {
            Manager.Accessibilty = true;
            Manager.AccessibiltyObject.SetActive(true);
        }
        else
        {
            Manager.Accessibilty = false;
            Manager.AccessibiltyObject.SetActive(false);
        }
        // Debug.Log("New Key Added : " + NewKey);
        KeyNew = NewKey;
    }

    // void SetAccessibilityByURL()
    // {
    //     string url = Application.absoluteURL;
					  
   
						  
   
	  
   
						   
   
  

    //     if (Application.absoluteURL.IndexOf("?") >= 0)
    //     {
    //         string[] accStrings = Application.absoluteURL.Split("?"[0])[1].Split("="[0]);
								 
											   
		  
								  
											   
   
										  
				  

    //         if (accStrings.Length == 2)
    //         {
    //             if ((accStrings[0] == "enableAccessibility") && (accStrings[1] == "true"))
    //             {
    //                 GetAccessibilty("true");
    //                 return;
    //             }
    //             Debug.Log(accStrings[0] + "=" + accStrings[1]);
    //         }
    //         Debug.Log(accStrings.ToString());
    //     }

    //     GetAccessibilty("false");
    // }
}

[Serializable]
public class GameStudentModel
{
    public int GameId { get; set; }
    public int StudentId { get; set; }
    public bool IsPreview { get; set; }
    // public bool EnableAccessibility;


    public int GetGameId()
    {
        return this.GameId;
    }

    public int GetStudentId()
    {
        return this.StudentId;
    }

    public bool GetAccessibiltyBool()
    {
        return this.IsPreview;
    }

    public void SetGameId(int ID)
    {
        this.GameId = ID;
    }

    public void SetStudentId(int StID)
    {
        this.StudentId = StID;
    }

    public void SetAccessibiltyBool(bool IsAccessible)
    {
        this.IsPreview = IsAccessible;
    }

    // public bool GetAccessibilty()
    // {
    //     return EnableAccessibility;
    // }

    // public void SetAccessibilty(bool EnableAccessibilty)
    // {
    //     this.EnableAccessibility = EnableAccessibilty;
    // }
}

