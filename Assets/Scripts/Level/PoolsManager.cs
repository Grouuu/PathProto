using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
	public string tag;
	public GameObject prefab;
	public Transform parent;
	public int size;
}

public class PoolsManager : MonoBehaviour
{
	public List<Pool> poolsConstruct;

	private Dictionary<string, Queue<GameObject>> pools;

	private void Start()
	{
		pools = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool poolModel in poolsConstruct)
		{
			Queue<GameObject> list = new Queue<GameObject>();

			for (int i = 0; i < poolModel.size; i++)
			{
				GameObject body = Instantiate(poolModel.prefab, poolModel.parent);
				body.SetActive(false);
				list.Enqueue(body);
			}

			pools.Add(poolModel.tag, list);
		}
	}

	public GameObject GetObject(string tag)
	{
		GameObject go = pools[tag].Dequeue();
		pools[tag].Enqueue(go);

		return go;
	}
}
