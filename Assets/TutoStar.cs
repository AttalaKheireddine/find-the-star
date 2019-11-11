using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoStar : MonoBehaviour {

	public GameObject boardObject;
	TutorialBoard board;

	public float moveTime,waitTime;
	Rigidbody2D rigid;
	public List<Vector2Int> moveSpots;

	void Start()
	{
		board = boardObject.GetComponent<TutorialBoard> ();
		rigid = GetComponent<Rigidbody2D> ();
		transform.position = board.getNodePos (0, 2);
	}

	public IEnumerator MakeMove()
	{
		foreach (Vector2Int spot in moveSpots) 
		{
			rigid.velocity = (board.getNodePos(spot.x,spot.y) - transform.position) / moveTime;
			yield return new WaitForSeconds (moveTime);
			rigid.velocity = Vector2.zero;
			yield return new WaitForSeconds (waitTime);

		}
	}
}
