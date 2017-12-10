using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class APanel : MonoBehaviour, IManagerInjector 
{
	public MainManager Man { get; set; }
	private CanvasGroup m_Panel;
	protected CanvasGroup Panel 
	{
		get
		{
			if (!m_Panel) m_Panel = GetComponent<CanvasGroup>();
			return m_Panel;
		}
	}

	protected virtual void Awake()
	{
		this.Inject();
	}

	public virtual void Show(bool show)
	{
		Panel.gameObject.SetActive(show);

		var firstSelectable = GetComponentsInChildren<Selectable>().FirstOrDefault();
		if (firstSelectable)
		{
			firstSelectable.Select();
		}

		//Meh..
		// Panel.alpha = show ? 1f : 0f;
		// Panel.interactable = show;
		// Panel.blocksRaycasts = show;
	}

}
