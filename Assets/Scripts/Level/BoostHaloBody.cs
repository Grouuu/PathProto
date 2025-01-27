using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostHaloBody : MonoBehaviour
{
	public GameObject halo;

	private void Awake()
	{
		halo.SetActive(false);
	}

	private void Update()
	{
		halo.transform.LookAt(Camera.main.transform.position, -Vector3.up);
	}

	public void ActivateHalo(float radiusPlanet, float width)
	{
		halo.SetActive(true);
		halo.transform.localScale = new Vector3(radiusPlanet + width, radiusPlanet + width, radiusPlanet + width);
	}

	public void DeactiveHalo()
	{
		halo.SetActive(false);
	}
}
