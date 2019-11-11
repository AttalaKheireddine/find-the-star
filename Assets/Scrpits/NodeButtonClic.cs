using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeButtonClic : MonoBehaviour {

	void Start()
	{
		GetComponent<Button> ().onClick.AddListener (OurOnClick);
	}
	public void OurOnClick()
	{
		GameObject.Find ("Board").GetComponent<GraphObject> ().HandleClicOnNode (transform.parent.parent.GetComponent<Node> ());
	}

}
