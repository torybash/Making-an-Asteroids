using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IPoolObject<T>
{
	private T m_Prefab;
	private int m_Capacity;
	private Stack<T> m_PoolObjects = new Stack<T>();
	private List<T> m_AliveObjects = new List<T>();
	
	private Transform m_Container;

	public Pool(T prefab, int capacity = 100) //TODO use capacity!
	{
		m_Prefab = prefab;
		m_Capacity = capacity;

		m_Container = new GameObject(typeof(T).Name + "_Pool").transform;
	}

	public T Get()
	{
		T instance = null;
		if (m_PoolObjects.Count > 0)
		{
			instance = m_PoolObjects.Pop();
			instance.gameObject.SetActive(true);
		}else
		{
			instance = GameObject.Instantiate(m_Prefab, m_Container); 
			instance.SetPool(this);
		}
		m_AliveObjects.Add(instance);

		return instance;
	}

	public void Return(T instance)
	{
		instance.gameObject.SetActive(false);
		m_Container.SetParent(m_Container);
		m_PoolObjects.Push(instance);
		if (m_AliveObjects.Contains(instance)) m_AliveObjects.Remove(instance);
	}

    public void ReturnAll()
    {
		var objects = new List<T>(m_AliveObjects);
		foreach (var obj in objects)
		{
			obj.Return();
		}
    }
}

public interface IPoolObject<T> where T : MonoBehaviour, IPoolObject<T>
{	
	Pool<T> Pool { get; set; }
	MonoBehaviour Behaviour { get; }
}

public static class IPoolObjectExtenstions
{
	public static void SetPool<T>(this IPoolObject<T> obj, Pool<T> pool) where T : MonoBehaviour, IPoolObject<T>
	{
		obj.Pool = pool;
	}
	public static void Return<T>(this IPoolObject<T> obj) where T : MonoBehaviour, IPoolObject<T>
	{
		obj.Pool.Return((T) obj.Behaviour);
	}
}