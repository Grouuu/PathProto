using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollFlamesManager : MonoBehaviour
{
	public Transform flames;
	public Transform player;
	public float speed = 25f;
	public float startOffsetX = 50f;

	private void Start()
	{
		Vector3 pos = flames.transform.position;
		pos.x = -startOffsetX;

		flames.transform.position = pos;
	}

	public void UpdateScroll()
	{
		Vector3 pos = flames.transform.position;
		pos.x += speed * Time.deltaTime;

		flames.transform.position = pos;
	}
}
