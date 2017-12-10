using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager : MonoBehaviour, IManagerInjector 
{
	public MainManager Man { get; set; }

	[SerializeField]
	private List<ParticleEffect> m_EffectsPrefabs;

	private Dictionary<string, Pool<ParticleEffect>> m_EffectPools = new Dictionary<string, Pool<ParticleEffect>>();


	void Awake()
	{
		this.Inject();

		foreach (var effectPrefab in m_EffectsPrefabs)
		{
			var pool = new Pool<ParticleEffect>(effectPrefab);
			m_EffectPools.Add(effectPrefab.name, pool);
		}
	}

	public void PlayEffect(string id, Vector2 pos)
	{
		if (!m_EffectPools.ContainsKey(id))
		{
			Debug.LogError("Effect " + id + " does not exits!");
			return;
		}
		var effectPool = m_EffectPools[id];
		var effect = effectPool.Get();
		effect.Init(pos);
	}
}
