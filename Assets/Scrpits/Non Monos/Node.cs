using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
	public Node leftNode=null;
	public Node rightNode=null;
	public Node upNode=null;
	public Node downNode=null;
	public GameObject rightConnection=null, upConnection=null;
	Color unmarkedColor;

	Vector2Int position;

	void Awake()
	{
		unmarkedColor = GetComponent<SpriteRenderer> ().color;
	}

	public Node GetNeighbour(Vector2Int vect)
	{
		if (vect == new Vector2Int(0,1))
			return upNode;
		else if (vect == new Vector2Int(0,-1))
			return downNode;
		else if (vect == new Vector2Int(1,0))
			return rightNode;
		else if (vect == new Vector2Int(-1,0))
			return leftNode;
		else
			throw new ArgumentException("wrong neighbour vector (ha zayyar jed ro7ek)");
	}
	public void SetNeighbour(Vector2Int vect,Node node)
	{
		if (vect == new Vector2Int(0,1))
			upNode=node;
		else if (vect == new Vector2Int(0,-1))
			downNode=node;
		else if (vect == new Vector2Int(1,0))
			rightNode=node;
		else if (vect == new Vector2Int(-1,0))
			leftNode=node;
		else
			throw new ArgumentException("wrong neighbour vector (ha zayyar jed ro7ek)");
		//if (leftNode == null)
		// if (rightNode == null)
		//  if (upNode == null)
		//   if (downNode == null)
			// destroy
	}

	public void DestroyConnection( Vector2Int vect )
	{
		GameObject objectToDestroy;
		if (vect == new Vector2Int (0, 1))
			objectToDestroy = upConnection;
		else if (vect == new Vector2Int(0,-1))
			objectToDestroy = GetNeighbour(new Vector2Int (0,-1)).upConnection;
		else if (vect == new Vector2Int(1,0))
			objectToDestroy = rightConnection;
		else if (vect == new Vector2Int(-1,0))
			objectToDestroy = GetNeighbour(new Vector2Int(-1,0)).rightConnection;
		else
			throw new ArgumentException("wrong neighbour vector (ha zayyar jed ro7ek)");
		if (objectToDestroy == null) {
			throw new NullReferenceException ();
		}
		Destroy (objectToDestroy);
	}
	public bool HasNoNeighbours()
	{
		if (upNode!=null) return false;
		if (downNode!=null) return false;
		if (leftNode!=null) return false;
		if (rightNode!=null) return false;
		return true;
	}

	public List<Vector2Int> PossibleDirections()
	{// returns direction where there exist neighbours
		List<Vector2Int> result = new List<Vector2Int>();
		if (leftNode != null)
			result.Add (new Vector2Int (-1, 0));
		if (rightNode != null)
			result.Add (new Vector2Int (1, 0));
		if (upNode != null)
			result.Add (new Vector2Int (0, 1));
		if (downNode != null)
			result.Add (new Vector2Int (0, -1));
		return result;
	}

	public void Mark(Color color)
	{
		GetComponent<SpriteRenderer> ().color = color;
	}

	public void Unmark()
	{
		GetComponent<SpriteRenderer> ().color = unmarkedColor;
	}
		

}

