using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
	public float delay = 0f;

	private void OnEnable()
	{
		gameObject.SetActive(true);

		if(delay < 0)
		{
			StartCoroutine(TimerClose());
		}
	}

	private IEnumerator TimerClose()
	{
		yield return new WaitForSeconds(delay);

		Destroy(gameObject);
	}
}
