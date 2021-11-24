using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BestHTTP;
using Newtonsoft;
using System;
using UnityEngine.Networking;
//using BestHTTP.Cookies;
//using BestHTTP.SignalR.Transports;
public class RestAPIHandler : MonoBehaviour {
	public static RestAPIHandler Instance;
	[HideInInspector]
	public bool UploadedSuccessfully=false;
//	private HTTPRequest AuthRequest;
//	private Cookie Cookie;

//	public bool IsPreAuthRequired { get; private set; }
//
//	public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;
//	public event OnAuthenticationFailedDelegate OnAuthenticationFailed;
	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	public void GET(Uri URL)
//	{
//		HTTPRequest request = new HTTPRequest(URL, OnRequestFinished);//new Uri("https://google.com")
//		request.Send();
//	}
//
//	public void POST(Uri URL,string JSON_body)
//	{
//		HTTPRequest request = new HTTPRequest(URL, HTTPMethods.Post, 
//			(req, response) => {
//				string respJSON = response.DataAsText;
//				onComplete(true,respJSON);
//			});
//		request.AddHeader("Content-Type", "application/json");
//		request.RawData = new System.Text.UTF8Encoding().GetBytes(JSON_body);
//		request.Send();
//	}
//	public void POST(Uri URL,string JSON_body)
//	{
//		AuthRequest = new HTTPRequest(URL, HTTPMethods.Post, OnAuthRequestFinished);
//		AuthRequest.AddHeader("Content-Type", "application/json");
//		AuthRequest.RawData = new System.Text.UTF8Encoding().GetBytes(JSON_body);
//		AuthRequest.Send();
//	}
//

//	//in main thread
//	public void onComplete(bool Bol, string json)
//	{
//		Debug.Log("Respone Finished! Text received: " + json);
//	}

//	public void OnRequestFinished(HTTPRequest request, HTTPResponse response)
//	{
//		Debug.Log("Respone Finished! Text received: " + response.DataAsText);
//		//Debug.Log("Request Finished! Text received: " + response.DataAsText);
//	}

	public IEnumerator PostRequest(string url, string json)
	{
		var uwr = new UnityWebRequest(url, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		uwr.SetRequestHeader("Content-Type", "application/json");

		//Send the request then wait here until it returns
		Debug.Log("waiting");
		yield return uwr.Send();
		UploadedSuccessfully = true;
		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
			Debug.Log("Received: " + uwr.downloadHandler.text);
		}
	}
//
//	void OnAuthRequestFinished(HTTPRequest req, HTTPResponse resp)
//	{
//		AuthRequest = null;
//		string failReason = string.Empty;
//		Debug.Log (resp.StatusCode +
//			resp.Message+
//			resp.DataAsText);
//		switch (req.State)
//		{
//		// The request finished without any problem.
//		case HTTPRequestStates.Finished:
//			if (resp.IsSuccess)
//			{
//				Cookie = resp.Cookies != null ? resp.Cookies.Find(c => c.Name.Equals(".ASPXAUTH")) : null;
//
//				if (Cookie != null)
//				{
//					HTTPManager.Logger.Information("CookieAuthentication", "Auth. Cookie found!");
//
////					if (OnAuthenticationSucceded != null)
////						OnAuthenticationSucceded(this);
//
//					// return now, all other paths are authentication failures
//					return;
//				}
//				else
//					HTTPManager.Logger.Warning("CookieAuthentication", failReason = "Auth. Cookie NOT found!");
//			}
//			else
//				HTTPManager.Logger.Warning("CookieAuthentication", failReason = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
//					resp.StatusCode,
//					resp.Message,
//					resp.DataAsText));
//			break;
//
//			// The request finished with an unexpected error. The request's Exception property may contain more info about the error.
//		case HTTPRequestStates.Error:
//			HTTPManager.Logger.Warning("CookieAuthentication", failReason = "Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
//			break;
//
//			// The request aborted, initiated by the user.
//		case HTTPRequestStates.Aborted:
//			HTTPManager.Logger.Warning("CookieAuthentication", failReason = "Request Aborted!");
//			break;
//
//			// Ceonnecting to the server is timed out.
//		case HTTPRequestStates.ConnectionTimedOut:
//			HTTPManager.Logger.Error("CookieAuthentication", failReason = "Connection Timed Out!");
//			break;
//
//			// The request didn't finished in the given time.
//		case HTTPRequestStates.TimedOut:
//			HTTPManager.Logger.Error("CookieAuthentication", failReason = "Processing the request Timed Out!");
//			break;
//		}
//
////		if (OnAuthenticationFailed != null)
////			OnAuthenticationFailed(this, failReason);
//	}

}
