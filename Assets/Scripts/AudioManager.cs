using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour , IManagerInjector 
{
	public MainManager Man { get; set; }

	[SerializeField]
	private List<AudioClip> m_Clips;

	[SerializeField]
	private SoundClip m_ClipPrefab;

	private Pool<SoundClip> m_ClipPool;


	void Awake()
	{
		this.Inject();

		m_ClipPool = new Pool<SoundClip>(m_ClipPrefab);
	}

	public void PlayClip(string id, bool looping = false)
	{
		var audioClip = m_Clips.FirstOrDefault(x => id == x.name);
		if (!audioClip)
		{
			Debug.LogError("AudioClip "+ id + " does not exits!");
			return;
		}
		var soundClip = m_ClipPool.Get();
		soundClip.Init(audioClip, looping);
	}

}
