using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassData : MonoBehaviour {
	static PassData instance=null;
	public int gridSize;
	public float initTimer;
	public GameObject gridSlider,timerSlider;

	// Use this for initialization

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		} 
		else 
		{
			Destroy (instance.gameObject);
		}
		instance = this;
		GameObject.DontDestroyOnLoad (gameObject);
	}
	public void GetData()
	{
		gridSize = (int)gridSlider.GetComponent<Slider> ().value;
		initTimer = timerSlider.GetComponent<Slider> ().value;
	}
}
