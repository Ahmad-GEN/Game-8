using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft;
using Newtonsoft.Json;

[Serializable]
public class Report
{
    #region variables
    private string gametitle; //stores the tittle of games
    private DateTime dateTime; // stores the date and the time of the game, on start
    private DateTime firstReportSentDateTime; // stores the date and the time of the game, on start

    private string associatedSkillTag;   //stores the skill related to games
    private float playTime; // totla time the game has taken to complete the games
    private int responsiveness; // total number of interation user did with games. include all the touhing and draging

    private double percentage; //perc of games the user has played in order to complete the game
    private string selectedMode;// which games mode user choosed to play.

    private string subtitle; // stores subtitle of games
    private bool hasTutorial;
    private int CorrectAttempts; // tell how much of your answers are correct.
    private int IncorrectAttempts;  // tell how much of your answers are wrong.

    public int reportsSentCount = 0;

    private List<Keys> selectedKeys; //detailed reporting view of correct letter and incorrect letter typed per session in reporting view 
    
    #endregion
    #region GetterAndSetter
    public string getGametitle()
    {
        return this.gametitle;
    }

    public void setGametitle(string gametitle)
    {
        this.gametitle = gametitle;
    }

    public DateTime getDatetime()
    {
        return this.dateTime;
    }

    public void setDatetime(DateTime dateTime)
    {
        this.dateTime = dateTime;
    }

    public string getAssociatedskilltag()
    {
        return this.associatedSkillTag;
    }

    public void setAssociatedskilltag(string associatedSkillTag)
    {
        this.associatedSkillTag = associatedSkillTag;
    }

    public float getPlaytime()
    {
        return this.playTime;
    }

    public void setPlaytime(float playTime)
    {
        this.playTime = playTime;
    }

    public int getResponsiveness()
    {
        return this.responsiveness;
    }

    public void setResponsiveness(int responsiveness)
    {
        this.responsiveness = responsiveness;
    }

    public double getPercentage()
    {
        return this.percentage;
    }

    public void setPercentage(double percentage)
    {
        this.percentage = percentage;
    }

    public string getSelectedmode()
    {
        return this.selectedMode;
    }

    public void setSelectedmode(string selectedMode)
    {
        this.selectedMode = selectedMode;
    }

    public string getSubtitle()
    {
        return this.subtitle;
    }

    public void setSubtitle(string subtitle)
    {
        this.subtitle = subtitle;
    }

    public void setFirstPostDateTime(DateTime firstReportSentDateTime)
    {
        this.firstReportSentDateTime = firstReportSentDateTime;
    }
	public DateTime GetFirstPostDateTime()
	{
		return this.firstReportSentDateTime;
	}


    public bool getHastutorial()
    {
        return this.hasTutorial;
    }

    public void isHastutorial(bool hasTutorial)
    {
        this.hasTutorial = hasTutorial;
    }

    public int getCorrectAttemps()
    {
        return this.CorrectAttempts;
    }

    public void setCorrectAttempts(int correctAtempts)
    {
        this.CorrectAttempts = correctAtempts;
    }

     public int getIncorrectAttempts()
    {
        return this.IncorrectAttempts;
    }

    public void setIncorrectAttempts(int IncorrectAttempts)
    {
        this.IncorrectAttempts = IncorrectAttempts;
    }

    public List<Keys> getSelectedkeys()
    {
        return this.selectedKeys;
    }

    public void setSelectedkeys(List<Keys> selectedKeys)
    {
        this.selectedKeys = selectedKeys;
    }
    #endregion
    #region function

    public void /// <summary>
                /// reset the report
                /// </summary>
        ResetReport()
    {
        gametitle = "";
        dateTime = System.DateTime.Now;
        associatedSkillTag = "";
        playTime = 0f;
        responsiveness = 0;
        percentage = 0f;
        selectedMode = "";
        subtitle = "";
        hasTutorial = false;
        CorrectAttempts = 0;
        IncorrectAttempts = 0;
        selectedKeys.Clear();

    }

//    public void PostData(string Url)
//    {
//        PlayerGameScore playerGameScore = new PlayerGameScore ();
//
//		playerGameScore.Complete = (int)percentage;
//		playerGameScore.CreatedBy= External.Instance.Model.GameId.ToString();
//		playerGameScore.CreatedDate = firstReportSentDateTime;
//		playerGameScore.Duration = (int)playTime;
//		playerGameScore.GameId = External.Instance.Model.GameId;
//		playerGameScore.IncorrectAttempts = IncorrectAttempts;
//		playerGameScore.InstanceId = 0; // Yet to be decided
//		playerGameScore.IsInAccessibilityMode = External.Instance.Model.IsPreview;
//		playerGameScore.ModifiedBy = External.Instance.Model.GameId.ToString();
//		playerGameScore.ModifiedDate = System.DateTime.UtcNow;
//		playerGameScore.PlayType = selectedMode;
//		playerGameScore.Responsiveness = responsiveness;
//		playerGameScore.StudentId = External.Instance.Model.StudentId;
//		playerGameScore.Timestamp = dateTime;
//
//		string JsonString = JsonConvert.SerializeObject (playerGameScore);
//		Debug.Log (JsonString);
//		RestAPIHandler.Instance.StartCoroutine(RestAPIHandler.Instance.PostRequest(Url,JsonString));
//    }


    #endregion

    [Serializable]
    public class Keys
    {
        public string key;
        public int value;
    }

//    public class PlayerGameScore
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
