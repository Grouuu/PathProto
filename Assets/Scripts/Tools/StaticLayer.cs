using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLayer : MonoBehaviour
{
	[HideInInspector]
	public static bool isCreated = false;

	private void Awake()
	{
		if (!isCreated)
		{
			DontDestroyOnLoad(transform.gameObject);

			isCreated = true;
		}
		else
			Destroy(gameObject);
	}
}
