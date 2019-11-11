using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeUpButtonActions : MonoBehaviour {

	public GameObject board;
	GraphObject everything;

	void Start()
	{
		everything = board.GetComponent<GraphObject> ();
	}

	// Use this for initialization
	public void Retry () 
	{
		
		everything.GameStart ();
	}

	public void Quit()
	{
		SceneManager.LoadScene (0);
	}
}
