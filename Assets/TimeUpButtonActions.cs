using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateScoreButtonActions : MonoBehaviour {

	public GameObject board;
	GraphObject everything;
	InputField input;

	void Start()
	{
		everything = board.GetComponent<GraphObject> ();
		input = GetComponentInChildren<InputField> ();
	}

	// Use this for initialization
	public void Retry () 
	{
		addScoreToRanking ();
		everything.GameStart ();
	}
	
	public void Quit()
	{
		addScoreToRanking ();
		SceneManager.LoadScene (0);
	}
	public void addScoreToRanking()
	{
		SizeAndTimer sat = everything.getCurrentSizeAndTime ();
		ScoreUpdatesHolda.rankingHolder.AddScore (everything.getScore (), sat.size, sat.timer, input.text);
	}
}
