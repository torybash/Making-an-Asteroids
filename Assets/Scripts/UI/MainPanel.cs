using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : APanel
{

	public void OnStartGame()
	{	
		Man.UI.HideMenus();
		Man.Game.StartGame();
	}

	public void OnOpenHighscores()
	{
		Man.UI.ShowHighscorePanel();
	}

	public void OnExit()
	{
		Application.Quit();
	}
}
