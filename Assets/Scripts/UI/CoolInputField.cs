using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class CoolInputField : MonoBehaviour 
{
	private InputField m_Field;
	private InputField Field 
	{
		get
		{
			if (!m_Field) m_Field = GetComponent<InputField>();
			return m_Field;
		}
	}

	[SerializeField]
	private Image m_CaretImage;

	void Start()
	{
		Field.ActivateInputField();
	}

	void Update()
	{
		var pos = Field.GetComponent<RectTransform>().anchoredPosition;
		// Field.car
		m_CaretImage.rectTransform.anchoredPosition = pos; 
	}
}
