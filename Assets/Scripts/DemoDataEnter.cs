using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft;
//using BestHTTP;
using Newtonsoft.Json;


public class DemoDataEnter : MonoBehaviour {
	public RestAPIHandler Ref;
	// Use this for initialization
	void Start () {
		//PostData ();
		//PostData("link");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
//	public void PostData(string Url)
//	{
//		PlayerGameScore p = new PlayerGameScore ();
//		p.Complete = 20;
//		p.CreatedBy="NyHumza";
//		p.CreatedDate.AddSeconds (99);
//		p.Duration = 60;
//		p.GameId = 66;
//		p.IncorrectAttempts = 200;
//		p.InstanceId = 990;
//		p.IsInAccessibilityMode = true;
//		p.ModifiedBy="HUMZA";
//		p.ModifiedDate.AddSeconds (33);
//		p.PlayType="HArd";
//		p.Responsiveness = 400;
//		p.StudentId = 23;
//		p.Timestamp.AddSeconds (11);
//		string JsonString = JsonConvert.SerializeObject (p);
//		Debug.Log (JsonString);
//		Ref.StartCoroutine(Ref.PostRequest(Url,JsonString));
//		//Ref.POST (new Uri ("https://l3skills.n2y-dev.com/api/GameAssetApi/GetGameAssets?gameId=102"), JsonString);
//	}

//public class PlayerGameScore
//{
//	public int? Complete { get; set; }
//	public string CreatedBy { get; set; }
//	public DateTime CreatedDate { get; set; }
//	public int Duration { get; set; }
//	public int GameId { get; set; }
//	public int? IncorrectAttempts { get; set; }
//	public int InstanceId { get; set; }
//	public  bool IsInAccessibilityMode { get; set; }
//	public string ModifiedBy { get; set; }
//	public DateTime ModifiedDate { get; set; }
//	public string PlayType { get; set; }
//	public int? Responsiveness { get; set; }
//	public int StudentId { get; set; }
//	public DateTime Timestamp { get; set; }
//}
}
