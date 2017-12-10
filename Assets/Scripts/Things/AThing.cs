using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AThing : MonoBehaviour, IManagerInjector 
{
	public MainManager Man { get; set; }
	private Rigidbody2D m_Body;
	protected Rigidbody2D Body 
	{
		get
		{
			if (!m_Body) m_Body = GetComponent<Rigidbody2D>();
			return m_Body;
		}
	}

	private Collider2D m_Collider;
	protected Collider2D Collider 
	{
		get
		{
			if (!m_Collider) m_Collider = GetComponent<Collider2D>();
			return m_Collider;
		}
	}

	private Renderer m_Renderer;
	protected Renderer Renderer 
	{
		get
		{
			if (!m_Renderer) m_Renderer = GetComponentInChildren<Renderer>();
			return m_Renderer;
		}
	}

	protected virtual void Awake()
	{
		this.Inject();
	}


	protected virtual void FixedUpdate()
	{
		// Debug.Log(this.ToString() + ", transform.position.x + Collider.bounds.extents.x: "+ (transform.position.x + Collider.bounds.extents.x));
		var newPos = transform.position;
		if (transform.position.x + Collider.bounds.extents.x > GameManager.LEVEL_WIDTH / 2f) newPos.x -= (GameManager.LEVEL_WIDTH - Collider.bounds.extents.x * 2f);
		else if (transform.position.x - Collider.bounds.extents.x < -GameManager.LEVEL_WIDTH / 2f) newPos.x += (GameManager.LEVEL_WIDTH - Collider.bounds.extents.x * 2f);
		else if (transform.position.y + Collider.bounds.extents.y > GameManager.LEVEL_HEIGHT / 2f) newPos.y -= (GameManager.LEVEL_HEIGHT - Collider.bounds.extents.y * 2f);
		else if (transform.position.y - Collider.bounds.extents.y < -GameManager.LEVEL_HEIGHT / 2f) newPos.y += (GameManager.LEVEL_HEIGHT - Collider.bounds.extents.y * 2f);
		if (transform.position != newPos) transform.position = newPos;
	}
}
