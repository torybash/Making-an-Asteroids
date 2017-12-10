using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AThing, IPoolObject<Bullet>
{
	public Pool<Bullet> Pool { get; set; }
	public MonoBehaviour Behaviour { get { return this; } }

	[SerializeField]
	private float m_LifeDuration;

	private float m_Deadtime;
	private AThing m_Owner;

	public bool IsPlayerBullet { get { return m_Owner.GetType() == typeof(Player); } }


    public void Init(Vector2 velocity, AThing owner)
    {
        Body.velocity = velocity;
		m_Owner = owner;
		m_Deadtime = Time.time + m_LifeDuration;
    }

	void Update()
	{
		if (Time.time > m_Deadtime)
		{
			this.Return();
		}
	}

    public void Explode()
    {
        this.Return();
    }
}
