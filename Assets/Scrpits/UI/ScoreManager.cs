using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	int score_=0;

	public int score
	{
		get
		{
			return score_;
		}
		set
		{
			score_ = value;
			GetComponent<Text> ().text = "Score : " + score;
		}
	}

	public void ResetScore()
	{
		score = 0;
	}
}
