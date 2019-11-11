using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRankings : MonoBehaviour {

	[SerializeField] Transform[] scoreRow;
	[SerializeField] int[] gridSizeValues;
	[SerializeField] float[] timerValues;
	// Use this for initialization
	void Start () {
		int zapdos = 0;
		int numberOfRows = scoreRow.Length;
		Dictionary<SizeAndTimer,NameAndScore> records = ScoreUpdatesHolda.rankingHolder.records;
		for (int gridSizeIndex = 0; gridSizeIndex < gridSizeValues.Length; gridSizeIndex++) {
			for (int timerIndex = 0; timerIndex < timerValues.Length; timerIndex++) 
			{
				int gridSize = gridSizeValues [gridSizeIndex];
				float timer = timerValues [timerIndex];
				Transform correctChild = scoreRow [timerIndex].GetChild (gridSizeIndex);
				try
				{
					NameAndScore nas = records [new SizeAndTimer (gridSize, timer)];
					correctChild.Find ("Name").GetComponent<Text> ().text = nas.name;
					correctChild.Find ("Score").GetComponent<Text> ().text = nas.score.ToString();
				}
				catch (KeyNotFoundException ex) 
				{
					zapdos++;
				}
			}
			Debug.Log (zapdos);
		}
	}
}
