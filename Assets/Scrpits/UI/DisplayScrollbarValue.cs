using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DisplayScrollbarValue : MonoBehaviour {

	public GameObject textToAdapt;
	public string textFormat = "_value_"; //we shall put that text in the textField, replacing every (_value_) with the slider value
	Text textField;
	Regex regex;
	Slider slider;
	// Use this for initialization
	void Start () {
		textField=textToAdapt.GetComponent<Text> ();
		regex = new Regex ("_value_");
		slider = GetComponent<Slider> ();
		OurOnValueChanged (slider.value);   // to initialize the value directly
		slider.onValueChanged.AddListener (OurOnValueChanged);
	}

	void OurOnValueChanged(float newValue)
	{
		textField.text = regex.Replace (textFormat, newValue.ToString ());
	}
}
