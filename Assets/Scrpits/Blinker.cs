using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour {

	public float onTime;
	public float offTime;

	Text text;
	// Use this for initialization
	void Start () 
	{
		text = GetComponent<Text> ();
		StartCoroutine ("Blink");
	}
	IEnumerator Blink()
	{
		for (;;) 
		{
			text.enabled = false;
			yield return new WaitForSeconds (offTime);
			text.enabled = true;
			yield return new WaitForSeconds (onTime);
		}
	}

}
