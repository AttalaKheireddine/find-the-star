using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDisplay : MonoBehaviour {

	public float verticalSpacing = 1;
	public float horizontalSpacing = 2;
	public int columnSize = 6;
	public Sprite upSprite,downSprite,leftSprite,rightSprite;
	public GameObject directionShowPrefab;
	public int maxChildren;

	// Use this for initialization
	void Awake () {
		for (int i = 0; i < maxChildren; i++) {
			GameObject instance =  Instantiate (directionShowPrefab, transform);
			instance.transform.localPosition = Vector3.zero;
		}
	}

	public void DisplayPath(Vector2Int[] path)
	{
		for (int i = 0; i < maxChildren; i++) 
		{
			transform.GetChild(i).gameObject.SetActive (i < path.Length);
			if (i < path.Length) 
			{
				transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = MapDirectionToSprite (path [i]);
				float x = (i/columnSize - 0.5f) * horizontalSpacing ;
				float y = - (i%columnSize - columnSize*0.5f) * verticalSpacing;
				transform.GetChild (i).localPosition = new Vector3 (x , y, 0);
			}
		}
	}

	public Sprite MapDirectionToSprite(Vector2Int direction)
	{
		if (direction == new Vector2Int (0, 1))
			return upSprite;
		if (direction == new Vector2Int (0, -1))
			return downSprite;
		if (direction == new Vector2Int (-1, 0))
			return leftSprite;
		if (direction == new Vector2Int (1, 0))
			return rightSprite;
		throw new ArgumentException ("wrong direction vector");
	}
}
