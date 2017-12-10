using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreEntry : MonoBehaviour, IPoolObject<HighscoreEntry>
{
	public Pool<HighscoreEntry> Pool { get; set; }
	public MonoBehaviour Behaviour { get { return this; } }


	[SerializeField]
	private Text m_PostionText;
	[SerializeField]
	private Text m_NameText;
	[SerializeField]
	private Text m_ScoreText;


	public void Init(int position, string name, int score)
	{
		m_PostionText.text = string.Format("{0}.", position);
		m_NameText.text = name;
		m_ScoreText.text = score.ToString();
	}
}
