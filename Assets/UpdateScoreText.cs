using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreText : MonoBehaviour {

	public GameObject scoreMan;

	// Use this for initialization
	void UpdateScore () {
		transform.Find ("Score Text").GetComponent<Text> ().text = "Score = " + scoreMan.GetComponent<ScoreManager> ().score;
	}
}
