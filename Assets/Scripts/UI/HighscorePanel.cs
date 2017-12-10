using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscorePanel : APanel 
{
	[SerializeField]
	private HighscoreEntry m_EntryPrefab;
	[SerializeField]
	private RectTransform m_HighscoresParent;

	private Pool<HighscoreEntry> m_EntryPool;
	private List<HighscoreEntry> m_Entries = new List<HighscoreEntry>();

	void Awake()
	{
		m_EntryPool = new Pool<HighscoreEntry>(m_EntryPrefab);
	}

	public override void Show(bool show)
	{
		base.Show(show);
		
		if (show)
		{
			foreach (var entry in m_Entries)
			{
				entry.Return();
			}

			for (int i = 0; i < Prefs.Entries.Count; i++)
			{
				var entryInfo = Prefs.Entries[i];
				var entry = m_EntryPool.Get();
				entry.Init(i + 1, entryInfo.Name, entryInfo.Score);
				entry.transform.SetParent(m_HighscoresParent);
				m_Entries.Add(entry);
			}
		}
	}
}
