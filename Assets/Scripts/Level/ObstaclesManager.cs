using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

enum Bodies
{
	Asteroid,
	Planet
}

public class CellData
{
	public int x;
	public int y;
	public GravityBody body;
}

public class ObstaclesManager : MonoBehaviour
{
	public float startImmunity = 100f;
	public string tagObstacle = "Planet";
	[Range(0f,1f)]
	public float freqBoost = 0.1f;
	public float widthBoostHalo = 1f;

	private GameManager gameManager;
	private List<CellData> listCells;

	private float sizeCell = 20f;
	private float marginCell = 5f;
	private int offsetCellSpawn = 5;
	private float minRadius = 2.5f; // minRadius * max difficulty <= maxRadius
	private float maxRadius = 5f;
	private float minDensity = 0.1f; // minDensity * max difficulty <= maxDensity
	private float maxDensity = 0.8f; // must be <= 1 (see GravityBody)

	private float difficulty;

	private void Start()
	{
		gameManager = GameManager.instance;
		listCells = new List<CellData>();
	}

	public void UpdateSpawnObstacles(Vector3 playerPos)
	{
		if (playerPos.x < startImmunity)
			return;

		difficulty = GameManager.instance.difficulty;

		Vector3 indexNearCell = GetCellIndex(playerPos);
		int playerNearCellX = (int)indexNearCell.x;
		int playerNearCellY = (int)indexNearCell.y;
		float randY = UnityEngine.Random.Range(-sizeCell, sizeCell);

		int cellX;
		int cellY;

		for (int i = -offsetCellSpawn; i < offsetCellSpawn; i++)
		{
			cellX = playerNearCellX + offsetCellSpawn;
			cellY = playerNearCellY + i;
			AddObstacle(cellX, cellY, randY);
		}

		CleanObstacles(playerNearCellX);
	}

	private Vector2 GetCellIndex(Vector3 pos)
	{
		Vector2 cellIndex = Vector2.zero;

		// not accurate (no rand Y) but ok
		cellIndex.x = (int)pos.x / sizeCell;
		cellIndex.y = (int)pos.y / sizeCell;

		return cellIndex;
	}

	private Vector3 GetCellPosition(int cellX, int cellY)
	{
		Vector3 cellPosition = Vector3.zero;

		cellPosition.x = cellX * sizeCell;
		cellPosition.y = cellY * sizeCell;

		return cellPosition;
	}

	private void AddObstacle(int cellX, int cellY, float randY)
	{
		if (isCellEmpty(cellX, cellY))
		{
			Vector3 test = GetCellPosition(cellX, cellY);
			
			Vector3 cellCenter = GetCellPosition(cellX, cellY) + new Vector3(sizeCell * .5f, sizeCell * .5f, 0); // + center offset
			cellCenter.y += randY;

			Vector3 randPos = new Vector3(	UnityEngine.Random.Range(-marginCell, marginCell),
											UnityEngine.Random.Range(-marginCell, marginCell), 0);

			Vector3 spawnPos = cellCenter + randPos;
			float radius = UnityEngine.Random.Range(minRadius * difficulty, maxRadius);
			float density = UnityEngine.Random.Range(minDensity * difficulty, maxDensity);
			bool isHalo = UnityEngine.Random.Range(0f, 1f) <= freqBoost;

			GravityBody body = SpawnAsteroid(tagObstacle, spawnPos, radius, density, isHalo);

			CellData data = new CellData();
			data.x = cellX;
			data.y = cellY;
			data.body = body;

			listCells.Add(data);
		}
	}

	private bool isCellEmpty(int cellX, int cellY)
	{
		foreach (CellData data in listCells)
		{
			if (data.x == cellX && data.y == cellY)
				return false;
		}

		return true;
	}

	private GravityBody SpawnAsteroid(string tag, Vector3 pos, float radius, float density, bool isHalo)
	{
		GameObject go = gameManager.poolsManager.GetObject(tag);
		go.SetActive(true);
		go.transform.position = pos;
		go.transform.rotation = UnityEngine.Random.rotation;

		GravityBody body = go.GetComponent<GravityBody>();

		body.SetRadius(radius);
		body.density = density;

		body.UpdateGravityWaves();
		
		BoostHaloBody halo = go.GetComponent<BoostHaloBody>();

		if (isHalo)
			halo.ActivateHalo(radius, widthBoostHalo);
		else
			halo.DeactiveHalo(); // needed because of recycle (?)

		return body;
	}

	private void CleanObstacles(int playerCellX)
	{
		CellData data;

		for (int i = listCells.Count - 1; i >= 0; i--)
		{
			data = listCells[i];

			if (data.x < playerCellX - 5)
			{
				listCells.RemoveAt(i);
			}
		}
	}
}
