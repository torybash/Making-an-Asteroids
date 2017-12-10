using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClip : MonoBehaviour, IPoolObject<SoundClip>
{
	public Pool<SoundClip> Pool { get; set; }
	public MonoBehaviour Behaviour { get { return this; } }

	private AudioSource m_Source;
	private AudioSource Source
	{
		get
		{ 
			if (!m_Source) m_Source = GetComponent<AudioSource>();
			return m_Source;
		}
	}
	

    public void Init(AudioClip audioClip, bool looping)
    {
        Source.clip = audioClip;
		Source.loop = looping;
		Source.Play();
		StartCoroutine(ReturnAfterPlay());
    }

	private IEnumerator ReturnAfterPlay()
	{
		while (Source.isPlaying) yield return null;
		this.Return();
	}
}