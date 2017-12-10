using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Random = UnityEngine.Random;

public static class Prefs 
{
	[Serializable]
	public class HighScoreEntries
	{
		public List<HighscoreEntry> Entries;
	}

	[Serializable]
	public class HighscoreEntry
	{
		public int Score;
		public string Name;
	}

	private static HighScoreEntries m_Entries;
	public static List<HighscoreEntry> Entries { get {return m_Entries.Entries; } set { m_Entries.Entries = value; } }
	
	private const int HIGHSCORE_ENTRY_COUNT = 10;
	

	public static bool IsNewHighscore(int score)
	{
		if (score == 0) return false;
		if (Entries.Count < HIGHSCORE_ENTRY_COUNT) return true;
		return Entries.Any(x => score > x.Score);
	}

	#if UNITY_EDITOR
	[MenuItem("Asteroids/Add test score")]
	public static void AddTestHighScore()
	{
		AddHighScore(Random.Range(200, 10000), "TSN");
	}
	#endif
	
	public static void AddHighScore(int score, string name)
	{
		Debug.Log("AddHighScore score: "+ score + ", name: "+ name);

		if (m_Entries == null) Load();
		
		var entry = new HighscoreEntry{ Score = score, Name = name };
		Entries.Add(entry);
		Entries = Entries.OrderByDescending(x => x.Score).ToList();

		if (Entries.Count > HIGHSCORE_ENTRY_COUNT) 
		{
			Entries = Entries.Take(HIGHSCORE_ENTRY_COUNT).ToList();
		}
		Save();
	}


	public static void Save()
	{
		PlayerPrefs.SetString("Entries", JsonUtility.ToJson(m_Entries));
		Debug.Log("Saving m_Entries: "+  JsonUtility.ToJson(m_Entries) + ", m_Entries.Count: " + Entries.Count);
	}

	public static void Load()
	{
		var entriesJson = PlayerPrefs.GetString("Entries", "");
		Debug.Log("Loading m_Entries - scoreJson "+  entriesJson + ", fromJson Count: "+ JsonUtility.FromJson<HighScoreEntries>(entriesJson).Entries.Count);
		m_Entries = new HighScoreEntries();
		Entries = string.IsNullOrEmpty(entriesJson) ? new List<HighscoreEntry>() : JsonUtility.FromJson<HighScoreEntries>(entriesJson).Entries;
	}
}


