using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreUpdatesHolda : MonoBehaviour
{
	public static ScoreUpdatesHolda instance = null;
	public static SaveData rankingHolder = new SaveData ();

	void Start()
	{
		if (instance != null) {
			Destroy (gameObject);
		} 
		else 
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
			rankingHolder.Load ();
		}
	}
}