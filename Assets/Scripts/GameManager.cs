using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour , IManagerInjector 
{
	public MainManager Man { get; set; }


	[SerializeField]
	private Player m_PlayerPrefab;
	[SerializeField]
	private Asteroid[] m_AsteroidPrefabs; 
	[SerializeField]
	private Bullet m_BulletPrefab;

	[SerializeField]
	private Collider2D[] m_SpawnAreas; 


	private Player m_Player;
	private List<Asteroid> m_Asteroids = new List<Asteroid>();

	private int m_CurrentLevel;

	private Transform m_ThingContainer;
	private Pool<Asteroid> m_AsteroidSmallPool;
	private Pool<Asteroid> m_AsteroidMediumPool;
	private Pool<Asteroid> m_AsteroidLargePool;
	private Pool<Bullet> m_BulletPool;

	[SerializeField]
	private Vector2 m_AsteroidSpeedRange = new Vector2(0.5f, 1f);

	private int m_Score;
	private int Score
	{
		get { return m_Score; }
		set
		{
			m_Score = value;
			Man.UI.UpdateScore(m_Score);
		}
	}
	private int m_Lives;
	private int Lives
	{
		get { return m_Lives; }
		set
		{
			m_Lives = value;
			Man.UI.UpdateLives(m_Lives);
		}
	}

	public const float LEVEL_WIDTH = 20f * 16f / 9f;
	public const float LEVEL_HEIGHT = 20f;
	public const int START_LIVES = 3;


	void Awake()
	{
		this.Inject();

		m_ThingContainer = new GameObject("Thing Container").transform;
		m_AsteroidSmallPool = new Pool<Asteroid>(m_AsteroidPrefabs[(int) AsteroidType.SMALL]);
		m_AsteroidMediumPool = new Pool<Asteroid>(m_AsteroidPrefabs[(int) AsteroidType.MEDIUM]);
		m_AsteroidLargePool = new Pool<Asteroid>(m_AsteroidPrefabs[(int) AsteroidType.LARGE]);
		m_BulletPool = new Pool<Bullet>(m_BulletPrefab);
	}

    public void Died()
    {
		Lives--;
        if (Lives > 0)
		{
			m_Player = Instantiate(m_PlayerPrefab, m_ThingContainer);
			m_Player.transform.position = GetSafePos();
		}else
		{
			GameOver();
		}
    }

	private void GameOver()
	{
		Debug.Log("GameOver - score: "+ Score);

		Man.UI.HideIngameUI();
		if (Prefs.IsNewHighscore(Score))
		{
			Man.UI.ShowNewScorePanel(Score);
		}else
		{
			Man.UI.ShowMainMenu();
		}
	}

    public void AsteroidKilled(Asteroid asteroid, int score)
    {
		if (m_Asteroids.Contains(asteroid)) m_Asteroids.Remove(asteroid);
		Score += score;
		Man.UI.UpdateScore(Score);

		if (m_Asteroids.Count == 0)
		{
			StartCoroutine(DoGoNextLevel());
		}
    }

	private IEnumerator DoGoNextLevel()
	{
		Debug.Log("DoGoNextLevel - level: " +(m_CurrentLevel + 1));
		m_CurrentLevel++;

		yield return new WaitForSeconds(1f);

		SpawnLevelAsteroids();
	}

    [ContextMenu("Start Game")]
	public void StartGame()
	{
		Reset();
		
		Score = 0;
		Lives = START_LIVES;
		m_CurrentLevel = 0;
		m_Player = Instantiate(m_PlayerPrefab, m_ThingContainer);
		
		SpawnLevelAsteroids();
	}

	private void Reset()
	{
		m_AsteroidSmallPool.ReturnAll();
		m_AsteroidMediumPool.ReturnAll();
		m_AsteroidLargePool.ReturnAll(); 
		m_BulletPool.ReturnAll();	
	}

	public void SpawnLevelAsteroids()
	{
		int amountAsteroids = 8 + Mathf.Clamp(m_CurrentLevel, 0, 8) * 2;
		for (int i = 0; i < amountAsteroids; i++)
		{
			//Select random spawn area
			var area  = m_SpawnAreas[Random.Range(0, m_SpawnAreas.Length)];
			SpawnAsteroid(AsteroidType.LARGE, GetRandomPosInArea(area), Random.insideUnitCircle * Random.Range(m_AsteroidSpeedRange.x, m_AsteroidSpeedRange.y));
		}
	}

	public void SpawnAsteroid(AsteroidType type, Vector2 pos, Vector2 velocity)
	{
		Pool<Asteroid> pool = null; 
		switch (type)
		{
			case AsteroidType.SMALL: 
				pool = m_AsteroidSmallPool;
				break;
			case AsteroidType.MEDIUM: 
				pool = m_AsteroidMediumPool;
				break;
			case AsteroidType.LARGE: 
				pool = m_AsteroidLargePool;
				break;
		}
		var asteroid = pool.Get();
		asteroid.transform.position = pos;
		asteroid.Init(velocity);
		m_Asteroids.Add(asteroid);
	}

	public void SpawnBullet(Vector2 pos, Vector2 velocity, AThing owner)
	{
		var bullet = m_BulletPool.Get();
		bullet.transform.position = pos;
		bullet.Init(velocity, owner);
	}


	private Vector2 GetRandomPosInArea(Collider2D area)
	{
		return new Vector2(Random.Range(area.bounds.min.x, area.bounds.max.x), Random.Range(area.bounds.min.y, area.bounds.max.y));
	}

	private Vector2 GetSafePos()
	{
		var pos = Vector2.zero;
		bool isSafe = false;
		float safeCheckDist = 1f;
		int derpCounter = 0;
		while (!isSafe && derpCounter < 1000)
		{
			pos = new Vector2(Random.Range(-LEVEL_WIDTH / 4f, LEVEL_WIDTH / 4f), Random.Range(-LEVEL_HEIGHT / 4f, LEVEL_HEIGHT / 4f));
			isSafe = m_Asteroids.All(x => Vector2.Distance(pos, x.transform.position) > safeCheckDist);
			safeCheckDist -= 0.1f;
			derpCounter++;
		}
		if (derpCounter == 1000) Debug.Log("DEEEEEEEEEEEEERP");

		return pos;
	}

}
