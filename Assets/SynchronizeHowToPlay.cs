using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SynchronizeHowToPlay : MonoBehaviour {

	public GameObject textObject,tutoStar,checkMark,boardObject;
	TutorialBoard board;
	ChangeText textChanger;
	public string[] textsToDisplay;
	public GameObject pathDisplay;
	public List<Vector2Int> path1, path2;
	public List<Vector2Int> nodesToMark;
	public Vector2Int checkmarkSpot1,checkmarkSpot2;

	// Use this for initialization
	void Start () 
	{
		board = boardObject.GetComponent<TutorialBoard> ();
		textChanger = textObject.GetComponent<ChangeText> ();
		StartCoroutine ("Play");
		StartCoroutine ("OtherThingsChange");
	}
	
	IEnumerator Play()
	{
		foreach (string text in textsToDisplay)
		{
			textChanger.Change (text);
			yield return NextClick ();
		}
		SceneManager.LoadScene (0);
	}

	IEnumerator OtherThingsChange()
	{
		yield return NextClick ();
		yield return NextClick ();
		tutoStar.GetComponent<TutoStar> ().StartCoroutine ("MakeMove");
		yield return NextClick ();
		yield return NextClick ();
		tutoStar.SetActive (false);
		checkMark.SetActive (true);
		checkMark.transform.position = board.getNodePos (checkmarkSpot1.x, checkmarkSpot1.y);
		yield return NextClick ();
		pathDisplay.GetComponent<PathDisplay> ().DisplayPath (path1.ToArray ());
		yield return NextClick ();
		yield return NextClick ();
		board.MarkNodes (nodesToMark.ToArray());
		pathDisplay.GetComponent<PathDisplay> ().DisplayPath (path2.ToArray ());
		checkMark.SetActive (false);
		yield return NextClick ();
		checkMark.SetActive (true);
		checkMark.transform.position =(board.getNodePos (checkmarkSpot2.x, checkmarkSpot2.y));
		yield return NextClick ();
	}

	IEnumerator NextClick()
	{
		yield return new WaitUntil(new System.Func<bool>(IsMousePressed));
		yield return new WaitForSeconds (0.1f);
	}

	bool IsMousePressed()
	{
		return Input.GetMouseButtonUp (0);
	}
}
