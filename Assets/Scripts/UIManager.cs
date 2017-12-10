using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManagerInjector 
{
	public MainManager Man { get; set; }




	////MENU
	[SerializeField]
	private Canvas m_MenuCanvas;
    [SerializeField]
	private IntroPanel m_IntroPanel;
    [SerializeField]
	private HighscorePanel m_HighscorePanel;
    [SerializeField]
	private MainPanel m_MainPanel;
    [SerializeField]
	private NewHighscorePanel m_NewHighscorePanel;

	////IN-GAME UI
	[SerializeField]
	private Canvas m_IngameCanvas;
    [SerializeField]
	private Text m_ScoreText;
	[SerializeField]
	private Text m_LivesText;

	private APanel m_CurrentPanel;


	void Awake()
	{
		this.Inject();

		m_HighscorePanel.Show(false);
		m_MainPanel.Show(false);
		m_NewHighscorePanel.Show(false);
	}

    public void UpdateScore(int score)
    {
        ShowCanvas(m_IngameCanvas);

        m_ScoreText.text = string.Format("Score: {0}", score);
    }

    public void UpdateLives(int lives)
    {
        ShowCanvas(m_IngameCanvas);

        m_LivesText.text = string.Format("Lives: {0}", lives);
    }

    public void HideMenus()
    {
		m_MenuCanvas.gameObject.SetActive(false);
		// m_MenuCanvas.enabled = false;
    }


    public void HideIngameUI()
    {
		m_IngameCanvas.gameObject.SetActive(false);
		// m_IngameCanvas.enabled = false;
    }

    public void ShowIntro()
    {
        ShowCanvas(m_MenuCanvas);
		ShowPanel(m_IntroPanel);
    }

    public void ShowMainMenu()
    {
        ShowCanvas(m_MenuCanvas);
		ShowPanel(m_MainPanel);
    }

    public void ShowHighscorePanel()
    {
        ShowCanvas(m_MenuCanvas);
		ShowPanel(m_HighscorePanel);
    }

    public void ShowNewScorePanel(int score)
    {
        ShowCanvas(m_MenuCanvas);
		ShowPanel(m_NewHighscorePanel);
		m_NewHighscorePanel.Init(score);
    }

    private void ShowCanvas(Canvas canvas)
    {
        if (!canvas.gameObject.activeSelf) canvas.gameObject.SetActive(true);
    }
    
	private void ShowPanel(APanel panel)
	{
		if (m_CurrentPanel) m_CurrentPanel.Show(false);
		m_CurrentPanel = panel;
		m_CurrentPanel.Show(true);
	}
}
