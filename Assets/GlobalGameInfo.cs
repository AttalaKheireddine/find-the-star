using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameInfo : ScriptableObject {
	public int minGridSize;
	public int maxGridSize;
	public int minTimer;
	public int maxTimer;

	void Awake()
	{
		GlobalGameInfo g;
	}

}
