using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public float initMinutes=1;
	//float initSeconds;  // they will be added and form the time
	public float timeStepInMillisecs = 100;
	float timeLeftInSeconds;
	public GameObject board;

	void Awake()
	{
		initMinutes  = GameObject.Find ("Data Passer").GetComponent<PassData> ().initTimer;
	}

	public void TimerStart()
	{
		//
		timeLeftInSeconds = initMinutes * 60;//+ initSeconds;
		StartCoroutine ("TimePassage");
	}


	IEnumerator TimePassage()
	{
		while (timeLeftInSeconds > 0) {
			int minutes = (int)timeLeftInSeconds / 60;
			float secs = timeLeftInSeconds - 60 * minutes;
			GetComponent<Text>().text = ("Time Left  "+minutes+":"+(int)secs+":"+(int)((secs - (int)secs)*100));
			timeLeftInSeconds -= (timeStepInMillisecs / 1000f);
			yield return new WaitForSeconds (timeStepInMillisecs/1000f);
		}
		TimeUp ();
	}

	void TimeUp()
	{
		board.GetComponent<GraphObject> ().GameEnd ();
	}
}
