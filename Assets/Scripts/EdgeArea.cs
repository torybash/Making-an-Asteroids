using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeArea : MonoBehaviour 
{

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Thing"))
		{
			var thing = collider.GetComponent<AThing>();
		}
	}

}
