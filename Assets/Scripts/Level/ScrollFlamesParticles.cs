using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollFlamesParticles : MonoBehaviour
{
	public GameObject flamesParticlesPrefab;
	public Transform parent;

	private int boxH = 100;

	private ParticleSystem.Particle[] points;
	private ParticleSystem system;
	private List<BoxData> listBox;

	private void Awake()
	{
		system = GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		listBox = new List<BoxData>();
	}

	public void UpdateParticles(Vector3 playerPos)
	{
		int playerBoxY = Mathf.FloorToInt(playerPos.y / boxH);

		AddBox(playerBoxY - 1);
		AddBox(playerBoxY);
		AddBox(playerBoxY + 1);

		CleanBoxes(playerBoxY);
	}

	public void AddBox(int indexY)
	{
		if (GetBoxByID(indexY) != null)
			return;

		float posX = parent.transform.localPosition.x;
		float posY = indexY * boxH;
		Vector3 boxPos = new Vector3(posX, posY, -10);

		GameObject box = Instantiate(flamesParticlesPrefab, boxPos, Quaternion.identity, parent);
		ParticleSystem ps = box.GetComponent<ParticleSystem>();

		BoxData data = new BoxData();
		data.ob = box;
		data.x = 0;
		data.y = indexY;

		listBox.Add(data);
	}

	private void CleanBoxes(int indexY)
	{
		List<BoxData> trash = new List<BoxData>();

		foreach (BoxData data in listBox)
		{
			if (Mathf.Abs(data.y - indexY) > 1)
				trash.Add(data);
		}

		foreach (BoxData data in trash)
		{
			Destroy(data.ob);
			listBox.Remove(data);
		}
	}

	private BoxData GetBoxByID(int indexY)
	{
		foreach (BoxData data in listBox)
		{
			if (data.y == indexY)
				return data;
		}

		return null;
	}
}
