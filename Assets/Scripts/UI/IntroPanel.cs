using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroPanel : APanel
{
	[SerializeField]
	private TextTyper m_TitleTyper;
	[SerializeField]
	private Text m_ContinueText;

	void Start()
	{
		m_ContinueText.gameObject.SetActive(false);
		m_TitleTyper.StartType();
		m_TitleTyper.OnComplete += OnTypeComplete;
	}

	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			Man.UI.ShowMainMenu();
			Man.Audio.PlayClip("through space", true);
		}
	}


	private void OnTypeComplete()
	{
		m_ContinueText.gameObject.SetActive(true);
	}
}
