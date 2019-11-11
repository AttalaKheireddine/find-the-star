using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphObject : MonoBehaviour {

	int size=2;
	public float visualSpacing = 1;   // spacing between the visual representation of the nodes
	public GameObject nodePrefab,connectionPrefab;
	public Color markedNodeColor;
	Node[,] nodeMatrix;
	bool clicsActive = false;
	Node correctNode;
	public GameObject scoreText;
	ScoreManager scoreManager;
	public GameObject timerObject,TimeUpCanvas,highScoreCanvas;
	public GameObject animatedStarPrefab,yesOrNoPrefab;
	public Sprite yesSprite, noSprite;
	Timer timer;
	Transform pathDisplay;
	public AudioClip yesSoundClip,noSoundClip,gameEndSound,highScoreSound;
	public GameObject audioSourceObject;
	AudioSource source;

	// Use this for initialization
	void Start () 
	{
		source = audioSourceObject.GetComponent<AudioSource> ();
		scoreManager = scoreText.GetComponent<ScoreManager> ();
		timer = timerObject.GetComponent<Timer> ();
		size = GameObject.Find ("Data Passer").GetComponent<PassData> ().gridSize;
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
		scoreManager.score = 0;
		timer.TimerStart ();
		InitializeMatrix ();
		RemoveConnections (size);
		TimeUpCanvas.SetActive (false);
		highScoreCanvas.SetActive (false);
		clicsActive = true;
		StartGamePhase ();
	}

	public void GameEnd()
	{
		clicsActive = false;
		if (! ScoreUpdatesHolda.rankingHolder.IsScoreMax (getScore(), size, timer.initMinutes)) {
			//high score in category
			source.PlayOneShot (gameEndSound);
			TimeUpCanvas.SetActive (true);
			TimeUpCanvas.SendMessage ("UpdateScore");
		} 
		else 
		{
			//other than high score, TODO = handle the score update UI and the actual score update
			source.PlayOneShot (gameEndSound);
			highScoreCanvas.SetActive (true);
			highScoreCanvas.SendMessage ("UpdateScore");
		}
	}

	public SizeAndTimer getCurrentSizeAndTime()
	{
		return new SizeAndTimer (size, timer.initMinutes);
	}

	public int getScore()
	{
		return scoreManager.score;
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

	void RemoveConnections(int number)
	{
		while (number > 0) {
			int i = Random.Range (0, size);
			int j = Random.Range (0, size);
			Vector2Int[] possibilities = { new Vector2Int (0, 1), new Vector2Int (0, -1), new Vector2Int (1, 0), new Vector2Int (-1, 0) };
			Vector2Int vecta = possibilities [Random.Range(0,4)];
			if ((nodeMatrix [i, j].GetNeighbour (vecta)) != null) {  // connection exists
				number--;
				nodeMatrix [i, j].DestroyConnection (vecta);
				nodeMatrix [i, j].GetNeighbour (vecta).SetNeighbour (new Vector2Int(-vecta.x,-vecta.y), null); //this is necessary cause the neighbour knows this one as his neighbour
				nodeMatrix [i, j].SetNeighbour (vecta, null);
			}
		}	
	}

	void StartGamePhase()
	{
		UnmarkAllNodes ();
		GenerateRandomPlay ();
		clicsActive = true;
	}

	public void HandleClicOnNode(Node node)
	{
		StartCoroutine ("ClicOperation",node);
	}

	IEnumerator ClicOperation(Node node)
	{
		if (!clicsActive)
			yield break;
		clicsActive = false;
		GameObject yesOrNoInstance = Instantiate (yesOrNoPrefab, node.transform.position, Quaternion.identity);
		yesOrNoInstance.GetComponent<SpriteRenderer> ().sprite = (node == correctNode) ? yesSprite : noSprite;
		if (node == correctNode) {
			scoreManager.score += 1;
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

	Vector2Int TracePath(Vector2Int startNode,Vector2Int[] path)
	// Update is called once per frame
	{
		Vector2Int currentPos = startNode;
		Node currentNode = nodeMatrix[startNode.x,startNode.y];
		foreach( Vector2Int vector in path)
		{
			if (currentNode == null) 
			{
				throw new ImpossiblePathException ();
			}
			currentNode = currentNode.GetNeighbour (vector);
			currentPos += vector;
		}
		if (currentNode == null) 
		{
			throw new ImpossiblePathException ();
		}
		return currentPos;
	}

	void GenerateRandomPlay()
	{
		List<Node> correctPathNodes = new List<Node> ();
		List<Vector2Int> possibleStartNodes = new List<Vector2Int> ();
		for (int i = 0; i < size; i++) 
		{
			for (int j = 0; j < size; j++)
			{
				possibleStartNodes.Add (new Vector2Int(i,j));
			}
		}
		Node correctStartNode;
		Vector2Int correctStartPosition;
		List<Vector2Int> correctPath = new List<Vector2Int> ();
		do 
		{
			correctStartPosition = new Vector2Int(Random.Range (0,size), Random.Range (0,size));
			correctStartNode = nodeMatrix [correctStartPosition.x,correctStartPosition.y];
		} while(correctStartNode.HasNoNeighbours ());
		correctPathNodes.Add (correctStartNode);
		bool stop = false;
		while (!stop) 
		{
			List<Vector2Int> nodesToRemove = new List<Vector2Int> ();
			foreach (Vector2Int nodePos in possibleStartNodes) {
				try
				{
					TracePath(nodePos,correctPath.ToArray());
				}
				catch (ImpossiblePathException ex) 
				{
					nodesToRemove.Add (nodePos);
				}
			}
			foreach (Vector2Int nodeR in nodesToRemove) {
				possibleStartNodes.Remove (nodeR);
			}
			if (possibleStartNodes.Count == 1) 
			{
				stop = true;
				break;
			}
			Vector2Int currentCorrectPosition = TracePath (correctStartPosition, correctPath.ToArray ());
			Node currentCorrectNode = nodeMatrix[currentCorrectPosition.x,currentCorrectPosition.y];
			List<Vector2Int> possibleWays = currentCorrectNode.PossibleDirections ();
			List<Vector2Int> possibleWays2 = new List<Vector2Int> ();
			foreach (Vector2Int way in possibleWays) {
				if (! correctPathNodes.Contains (currentCorrectNode.GetNeighbour (way))) {
					possibleWays2.Add (way);
				}
			}

			possibleWays = possibleWays2;

			if (possibleWays.Count == 0) {
				foreach (Vector2Int place in possibleStartNodes) {
					if (place != correctStartPosition) {
						MarkNode (new Vector2Int (place.x, place.y));
					}
				}
				stop = true;
			} 
			else 
			{
				Vector2Int chosenWay = possibleWays [Random.Range (0, possibleWays.Count)];
				correctPath.Add (chosenWay);
				correctPathNodes.Add (currentCorrectNode.GetNeighbour (chosenWay));
			}
		}

		Vector2Int correctEnd = TracePath (correctStartPosition, correctPath.ToArray());
		correctNode = nodeMatrix [correctEnd.x, correctEnd.y];
		GetComponentInChildren<PathDisplay> ().DisplayPath (correctPath.ToArray ());
	}

	void MarkNode(Vector2Int position)
	{
		nodeMatrix [position.x, position.y].Mark (markedNodeColor);
	}

	void MarkNodes(Vector2Int[] positions)
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
