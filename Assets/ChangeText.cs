using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {

	Text text;
	// Use this for initialization
	void Awake () {
		text = GetComponent<Text> ();
	}
	
	public void Change( string newText)
	{
		text.text = newText;
	}
}
