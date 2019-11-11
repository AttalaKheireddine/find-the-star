using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void Play()
	{
		GameObject.FindGameObjectWithTag ("Pass Data").GetComponent<PassData> ().GetData ();
		SceneManager.LoadScene (1);
	}
	public void Quit()
	{
		Application.Quit ();
	}
	public void ToRanking()
	{
		SceneManager.LoadScene (2);
	}

	public void ToTutorial()
	{
		SceneManager.LoadScene (3);
	}
}
