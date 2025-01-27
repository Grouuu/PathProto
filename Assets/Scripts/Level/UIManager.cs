using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text distanceText;

	private void Start()
	{
		distanceText.text = "";
	}

	public void UpdateDistance(float distance)
	{
		if (distance < 0)
			return;

		distanceText.text = distance.ToString("0");
	}
}
