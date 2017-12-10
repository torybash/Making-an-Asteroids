using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour 
{

	void Start()
	{
		Prefs.Load();
		UI.ShowIntro();
		UI.HideIngameUI();
	}


	private GameManager m_Game;
	public GameManager Game
	{
		get
		{
			if (!m_Game) m_Game = GetComponent<GameManager>();
			return m_Game;
		}
	}

	private AudioManager m_Audio;
	public AudioManager Audio
	{
		get
		{
			if (!m_Audio) m_Audio = GetComponent<AudioManager>();
			return m_Audio;
		}
	}

	private UIManager m_UI;
	public UIManager UI
	{
		get
		{
			if (!m_UI) m_UI = GetComponent<UIManager>();
			return m_UI;
		}
	}

	private EffectManager m_Effects;
	public EffectManager Effects
	{
		get
		{
			if (!m_Effects) m_Effects = GetComponent<EffectManager>();
			return m_Effects;
		}
	}
}

public interface IManagerInjector
{
	MainManager Man { get; set; }
}

public static class ManagerInjectorExtenstions
{
	public static void Inject(this IManagerInjector injector)
	{
		injector.Man = GameObject.FindGameObjectWithTag("Managers").GetComponent<MainManager>(); 
	}
}