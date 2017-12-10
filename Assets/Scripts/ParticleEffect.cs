using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour, IPoolObject<ParticleEffect>
{
	public Pool<ParticleEffect> Pool { get; set; }
	public MonoBehaviour Behaviour { get { return this; } }

	private ParticleSystem m_System;
	private ParticleSystem System
	{
		get
		{ 
			if (!m_System) m_System = GetComponent<ParticleSystem>();
			return m_System;
		}
	}
	

    public void Init(Vector2 pos)
    {
		this.transform.position = pos;
		System.Play();
		StartCoroutine(ReturnAfterPlay());
    }

	private IEnumerator ReturnAfterPlay()
	{
		while (System.isPlaying) yield return null;
		this.Return();
	}
}
