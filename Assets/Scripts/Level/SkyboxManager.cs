using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
	public float delay = 1f;
	public Color skyboxColor;

	private Material skyboxMaterial;

	private Color startColor;
	private float time = 0f;

    public void Awake()
	{
		skyboxMaterial = RenderSettings.skybox;
		startColor = skyboxMaterial.GetColor("_Tint");
	}

	private void Update()
	{
		if (time < delay)
		{
			skyboxMaterial.SetColor("_Tint", Color.Lerp(startColor, skyboxColor, time / delay));
			time = Time.timeSinceLevelLoad;
		}
	}

	private void OnDisable()
	{
		skyboxMaterial.SetColor("_Tint", startColor);
	}
}
