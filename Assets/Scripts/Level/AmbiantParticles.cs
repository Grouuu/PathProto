using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO :
// - STARS
// BUG : les box se remove de plus en plus en avant du player
// NOTE : pk boxW * 2 pour avoir en visuel une box pleine ?

public class AmbiantParticles : MonoBehaviour
{
	public GameObject ambiantParticlesPrefab;
	public Transform parent;
	public int maxStars = 1000;
	public float minSize = 0.1f;
	public float maxSize = 0.5f;
	public Color color = Color.white;
	
	private int boxW = 100;
	private int boxH = 100;
	private int depth = 50;

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
		int playerBoxX = Mathf.FloorToInt(playerPos.x / boxW);
		int playerBoxY = Mathf.FloorToInt(playerPos.y / boxH);

		AddBox(playerBoxX - 1, playerBoxY);
		AddBox(playerBoxX, playerBoxY);
		AddBox(playerBoxX + 1, playerBoxY);

		AddBox(playerBoxX - 1, playerBoxY + 1);
		AddBox(playerBoxX, playerBoxY + 1);
		AddBox(playerBoxX + 1, playerBoxY + 1);

		AddBox(playerBoxX - 1, playerBoxY - 1);
		AddBox(playerBoxX, playerBoxY - 1);
		AddBox(playerBoxX + 1, playerBoxY - 1);

		CleanBoxes(playerBoxX, playerBoxY);
	}

	public void AddBox(int indexX, int indexY)
	{
		if (GetBoxByID(indexX, indexY) != null)
			return;

		float posX = indexX * boxW;
		float posY = indexY * boxH;
		Vector3 boxPos = new Vector3(posX, posY, depth);

		GameObject box = Instantiate(ambiantParticlesPrefab, boxPos, Quaternion.identity, parent);
		ParticleSystem ps = box.GetComponent<ParticleSystem>();

		BoxData data = new BoxData();
		data.ob = box;
		data.x = indexX;
		data.y = indexY;

		listBox.Add(data);

		SetParticles(ps);
	}

	public void SetParticles(ParticleSystem ps)
	{
		points = new ParticleSystem.Particle[maxStars];

		float posX;
		float posY;
		float posZ;

		for (int i = 0; i < maxStars; i++)
		{
			posX = Random.Range(0, boxW);
			posY = Random.Range(0, boxH);
			posZ = Random.Range(1, 50);

			points[i].position = new Vector3(posX, posY, posZ);
			points[i].startSize = Random.Range(minSize, maxSize);
			points[i].startColor = color;
		}

		ps.SetParticles(points, points.Length);
	}

	private void CleanBoxes(int indexX, int indexY)
	{
		List<BoxData> trash = new List<BoxData>();

		foreach (BoxData data in listBox)
		{
			if (Mathf.Abs(data.x - indexX) > 1 || Mathf.Abs(data.y - indexY) > 1)
				trash.Add(data);
		}

		foreach (BoxData data in trash)
		{
			Destroy(data.ob);
			listBox.Remove(data);
		}
	}

	private BoxData GetBoxByID(int indexX, int indexY)
	{
		foreach (BoxData data in listBox)
		{
			if (data.x == indexX && data.y == indexY)
				return data;
		}

		return null;
	}
}
