using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : AThing, IPoolObject<Asteroid>
{
	public Pool<Asteroid> Pool { get; set; }
	public MonoBehaviour Behaviour { get { return this; } }


	[SerializeField]
	private AsteroidType m_Type;
	[SerializeField]
	private int m_Score = 100;
	[SerializeField]
	private Vector2 m_OffshootSpeedFractionRange = new Vector2(0.4f, 1f);
	[SerializeField]
	private Vector2 m_VisualAngularSpeed = new Vector2(0.4f, 1f);

	[SerializeField]
	private string m_ExplodeSoundId = "explosion-1";

	[SerializeField]
	private Transform m_Visuals;
	private Vector3 m_VisualAngularVelocity;


	void Update()
	{
		m_Visuals.Rotate(m_VisualAngularVelocity * Time.deltaTime); 
	}

	public void Init(Vector2 velocity)
	{
		Body.velocity = velocity;
		m_VisualAngularVelocity = Random.insideUnitSphere * Random.Range(m_VisualAngularSpeed.x, m_VisualAngularSpeed.y);
	}

	private void Explode()
	{
		Man.Audio.PlayClip(m_ExplodeSoundId);
		Man.Effects.PlayEffect("AsteroidExplosion", this.transform.position);

		if (m_Type != AsteroidType.SMALL)
		{
			var spawnType = m_Type - 1;
			var offsetVel = new Vector2(Body.velocity.y, Body.velocity.x) * Random.Range(m_OffshootSpeedFractionRange.x, m_OffshootSpeedFractionRange.y);;
			Man.Game.SpawnAsteroid(spawnType, transform.position + transform.right * Collider.bounds.extents.x, Body.velocity + offsetVel);
			Man.Game.SpawnAsteroid(spawnType, transform.position - transform.right * Collider.bounds.extents.x, Body.velocity - offsetVel);
		} 
		this.Return();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Bullet"))
		{
			Debug.Log("asteroid hit by " + collider);
			Explode();

			var bullet = collider.GetComponent<Bullet>();
			bullet.Explode();

			if (bullet.IsPlayerBullet)
			{
				Man.Game.AsteroidKilled(this, m_Score);
			}
		}
	
		
	}
}


public enum AsteroidType : int
{
	SMALL,
	MEDIUM,
	LARGE,
}