using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHighscorePanel : APanel 
{
	[SerializeField]
	private Text m_ScoreText;
	[SerializeField]
	private InputField m_NameInput;
	[SerializeField]
	private Button m_AcceptButton;

	private int m_Score;

	public void Init(int score)
	{
		m_Score = score;
		m_ScoreText.text = string.Format("YOU SCORED {0}", score);
		m_NameInput.text = "";
		m_AcceptButton.interactable = false;
	}

	public void OnEditName(string newName)
	{
		m_NameInput.text = newName.ToUpper();
		m_AcceptButton.interactable = newName.Length > 0;
	}
	
	public void OnAcceptName()
	{
		Prefs.AddHighScore(m_Score, m_NameInput.text);
		Man.UI.ShowHighscorePanel();
	}
}
