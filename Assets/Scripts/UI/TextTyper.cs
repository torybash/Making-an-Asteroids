using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour, IManagerInjector
{
	public MainManager Man { get; set; }


	[SerializeField]
	private Text m_TitleText;

	[SerializeField]
	private float m_CharactersPerSecond = 50f;

	private string m_Title;



    [SerializeField]
	private string m_OnFinishSoundId = "cure";
    public Action OnComplete;

    void Awake()
	{
		this.Inject();
		m_Title = m_TitleText.text;
		m_TitleText.text = "";
	}

	public void StartType()
	{
		StartCoroutine(DoTypeText());
	}

	private IEnumerator DoTypeText()
	{
		Debug.Log("TypingText:\n" + m_Title);

		int charCount = 0;
		while (m_Title.Length != m_TitleText.text.Length)
		{
			int charsToWriteCount = (int) (Time.deltaTime * m_CharactersPerSecond);
			charCount = Mathf.Clamp(charCount + charsToWriteCount, 0, m_Title.Length);
			m_TitleText.text = m_Title.Substring(0, charCount);

			yield return null;
		}

		if (OnComplete != null) OnComplete();
		Man.Audio.PlayClip(m_OnFinishSoundId);	
	}

}
