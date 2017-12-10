using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AThing 
{
	private Vector2 m_Input;

	[SerializeField]
	private float m_ThrusterSpeed = 1f;
	[SerializeField]
	private float m_RotationSpeed = 1f;
	[SerializeField]
	private float m_BulletSpawnOffset = 0.25f;
	[SerializeField]
	private float m_BulletSpeed = 1f;
	[SerializeField]
	private float m_FireCooldown = 0.25f;

	[SerializeField]
	private float m_VisualRotationMultiplier = 60f;
	[SerializeField]
	private Transform m_Visuals;
	[SerializeField]
	private ParticleSystem m_ThrusterSystem;

	private float m_NextFireTime;

	private bool m_IsDead;

	void Update()
	{
		if (m_IsDead) return;

		m_Input.x = Input.GetAxis("Horizontal");
		m_Input.y = Mathf.Clamp01(Input.GetAxis("Vertical"));

		if (Input.GetButton("Jump"))
		{
			if (Time.time > m_NextFireTime)
			{
				Man.Game.SpawnBullet(transform.position + transform.up * m_BulletSpawnOffset, m_BulletSpeed * transform.up, this);
				m_NextFireTime = Time.time + m_FireCooldown;	

				Man.Audio.PlayClip("shoot-1");		
			}
		}

		UpdateThrusters();
		m_Visuals.localRotation = Quaternion.Euler(0, m_Input.x * m_VisualRotationMultiplier, 0); 
	}

	private void UpdateThrusters()
	{
		var em = m_ThrusterSystem.emission;
		em.enabled = m_Input.y > 0;
		var main = m_ThrusterSystem.main;
		main.startSize = new ParticleSystem.MinMaxCurve(m_Input.y * 0.8f, m_Input.y * 1.15f);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		Body.AddForce(m_Input.y * transform.up * m_ThrusterSpeed);
		Body.angularVelocity = m_Input.x * m_RotationSpeed;
		// Body.AddTorque(m_Input.x * m_RotationSpeed);
	}


	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("collider: "+ collider);
	
		StartCoroutine(DoDie());
	}


	private IEnumerator DoDie()
	{
		Debug.Log("Dead!");

		Man.Audio.PlayClip("hit");
		Man.Effects.PlayEffect("Explosion", this.transform.position);

		m_IsDead = true;
		m_Visuals.gameObject.SetActive(false);
		
		yield return new WaitForSeconds(0.5f);

		Man.Audio.PlayClip("death");
		Man.Game.Died();

		Destroy(gameObject);
	}
}
