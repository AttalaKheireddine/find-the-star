using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class SaveData 
{
	[XmlRoot("RANKING")]
	public class RankingData
	{
		public List<Vector3> sizeTimeScores = new List<Vector3> ();
		public List<string> names = new List<string>();
		public RankingData()
		{}
	}
	public Dictionary<SizeAndTimer,NameAndScore> records = new Dictionary<SizeAndTimer, NameAndScore> ();
	public int minGridSize=3, maxGridSize=8;
	public float minTimer=1, maxTimer=5;
	public SaveData()
	{//constructor, we only need to save in order to init the file
		try
		{
			Load();
		}
		catch (FileNotFoundException) 
		{
			ranking = new RankingData ();
			for (int size = minGridSize; size <= maxGridSize; size++) {
				for (float time = minTimer; time <= maxTimer; time++) {
					records.Add (new SizeAndTimer (size, time), new NameAndScore());
				}
			}
				
			Save ();
		}
	}
	public RankingData ranking = new RankingData ();
	public void Save(string fileName = "zapdos.xml")
	{
		ranking = new RankingData ();
		foreach( SizeAndTimer sat in records.Keys)
		{
			ranking.sizeTimeScores.Add (new Vector3 (sat.size, sat.timer, records [sat].score));
			ranking.names.Add (records [sat].name);
		}
		XmlSerializer seria = new XmlSerializer (typeof(RankingData));
		FileStream stream = new FileStream (fileName, FileMode.OpenOrCreate);
		seria.Serialize (stream, ranking);
		stream.Close ();
	}
	public void Load(string fileName = "zapdos.xml")
	{
		XmlSerializer seria = new XmlSerializer (typeof(RankingData));
		FileStream stream = new FileStream (fileName, FileMode.Open);
		ranking= seria.Deserialize (stream) as RankingData;
		records = new Dictionary<SizeAndTimer, NameAndScore> ();
		stream.Close ();
		for(int i=0;i<ranking.sizeTimeScores.Count;i++)
		{
			Vector3 sizeTimeScore = ranking.sizeTimeScores[i];
			records.Add (new SizeAndTimer ((int)sizeTimeScore.x, sizeTimeScore.y), new NameAndScore( ranking.names[i],(int)sizeTimeScore.z));
		}

	}
	public bool IsScoreMax(int score,int size, float timer)
	{
		return records [new SizeAndTimer (size, timer)].score < score;
	}

	public void AddScore(int score, int size,float timer,string name)
	{//tries to add the player data,will throw an exception if the score is too low
		records[new SizeAndTimer(size,timer)] = new NameAndScore(name,score);
		Save ();
	}

}

