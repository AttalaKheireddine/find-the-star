using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoard : MonoBehaviour {

	int size=3;
	public float visualSpacing = 1;   // spacing between the visual representation of the nodes
	public GameObject nodePrefab,connectionPrefab;
	public Color markedNodeColor;
	Node correctNode;
	Node[,] nodeMatrix;
	public GameObject animatedStarPrefab,yesOrNoPrefab;
	public Sprite yesSprite, noSprite;
	Timer timer;
	Transform pathDisplay;
	public AudioClip yesSoundClip,noSoundClip;
	public GameObject audioSourceObject;
	AudioSource source;

	// Use this for initialization
	void Awake() 
	{
		source = audioSourceObject.GetComponent<AudioSource> ();
		pathDisplay = transform.GetChild (0);
		GameStart ();
	}

	public void GameStart()
	{

		foreach (Transform t in transform) {
			if (t != pathDisplay) {
				Destroy (t.gameObject);
			}
		}
		InitializeMatrix ();
		RemoveConnections ();
		StartGamePhase ();
	}

	void RemoveConnections()
	{
		Destroy(nodeMatrix [0, 0].rightConnection);
		Destroy(nodeMatrix [1, 0].rightConnection);
		Destroy(nodeMatrix [1, 1].upConnection);
		Destroy(nodeMatrix [2, 1].upConnection);
		Destroy(nodeMatrix [0, 2].rightConnection);
	}

	public Vector3 getNodePos(int x,int y)
	{
		return nodeMatrix [x, y].transform.position;
	}

	void InitializeMatrix(int? theSize = null)
	{
		if (theSize != null) {
			size = (int)theSize;
		}
		nodeMatrix =new Node [size, size];
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				GameObject nodeObj = Instantiate (nodePrefab, gameObject.transform);
				nodeObj.transform.localPosition = new Vector3 ((-((float)size)/2+i) * visualSpacing, (-((float)size)/2+j) * visualSpacing, 0);
				nodeMatrix [i, j] = nodeObj.GetComponent<Node>();
				if (i != 0)
				{
					nodeMatrix [i,j].leftNode = nodeMatrix [i - 1,j];
					nodeMatrix [i - 1,j].rightNode = nodeMatrix [i,j];
					GameObject connector = Instantiate (connectionPrefab, gameObject.transform);
					nodeMatrix [i - 1, j].rightConnection = connector;
					connector.transform.localPosition = new Vector3 ((-((float)size)/2+i-0.5f) * visualSpacing, (-((float)size)/2+j) * visualSpacing, 0);
					//visualConnections.Add (new Vector2Int (i, j), connector);
				}
				if (j != 0)
				{
					nodeMatrix [i,j].downNode = nodeMatrix [i,j - 1];
					nodeMatrix [i,j - 1].upNode = nodeMatrix [i,j];
					GameObject connector = Instantiate (connectionPrefab, gameObject.transform);
					nodeMatrix [i , j - 1].upConnection = connector;
					connector.transform.localPosition = new Vector3 ((-((float)size)/2+i) * visualSpacing, (-((float)size)/2+j-0.5f) * visualSpacing, 0);
					connector.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 90));
					//visualConnections.Add (new Vector2Int (i, j), connector);
				}
			}
		}
	}

	void StartGamePhase()
	{
		UnmarkAllNodes ();
	}

	public void HandleClicOnNode(Node node)
	{
		StartCoroutine ("ClicOperation",node);
	}

	IEnumerator ClicSimulation(Node node)
	{
		GameObject yesOrNoInstance = Instantiate (yesOrNoPrefab, node.transform.position, Quaternion.identity);
		yesOrNoInstance.GetComponent<SpriteRenderer> ().sprite = (node == correctNode) ? yesSprite : noSprite;
		if (node == correctNode) {
			source.PlayOneShot (yesSoundClip);
		} 
		else 
		{
			source.PlayOneShot (noSoundClip);
		}
		yield return StarAnimation (correctNode.transform.position);
		Destroy (yesOrNoInstance);
		StartGamePhase ();
	}

	IEnumerator StarAnimation(Vector3 position)
	{
		GameObject instance = Instantiate (animatedStarPrefab, position, Quaternion.identity);
		yield return new WaitForSeconds (instance.GetComponentInChildren<Animation> ().clip.averageDuration);
		Destroy (instance);
	}

	void MarkNode(Vector2Int position)
	{
		nodeMatrix [position.x, position.y].Mark (markedNodeColor);
	}

	public void MarkNodes(Vector2Int[] positions)
	{
		foreach (Vector2Int position in positions) {
			nodeMatrix [position.x, position.y].Mark (markedNodeColor);
		}
	}

	void UnmarkAllNodes()
	{
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				nodeMatrix [i, j].Unmark ();
			}
		}
	}
}
