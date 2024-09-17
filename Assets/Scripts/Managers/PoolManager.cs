using UnityEngine;
using System.Collections.Generic;
using System;

public class PoolManager : MonoBehaviour
{
	private Dictionary<Type, List<GameObject>> pooledGameobjects = new Dictionary<Type, List<GameObject>> ();

	public GameObject GetInstanciedPrefab(GameObject prefab, Vector3 position, Type type)
	{
        if (pooledGameobjects.ContainsKey(type))
        {
			List<GameObject> list = pooledGameobjects[type];
			if (list.Count > 0)
			{
				GameObject instanciedGameobject = list[0];
				list.RemoveAt(0);
				instanciedGameobject.gameObject.SetActive(true);
				instanciedGameobject.transform.position = position;
				pooledGameobjects[type] = list;

				return instanciedGameobject;
			}
		}

		CreateOrFeedPool(prefab, 10, type);
		return GetInstanciedPrefab(prefab, position, type);
	}

	public void RecycleItem(GameObject item, Type type)
	{
		Debug.Log(type);
		List<GameObject> list = pooledGameobjects[type];
		list.Add(item);
		pooledGameobjects[type] = list;
		item.gameObject.SetActive(false);
	}

	public void CreateOrFeedPool(GameObject prefab, int amount, Type type)
	{
		if(pooledGameobjects.ContainsKey(type))
		{
			List<GameObject> list = pooledGameobjects[type];

			for (int current = 0; current < amount; current++)
			{
				AddCreatedGameobjectToList(ref list, prefab);
			}
			pooledGameobjects[type] = list;
		}
		else
		{
			List<GameObject> list = new List<GameObject>();
			for (int current = 0; current < amount; current++)
			{
				AddCreatedGameobjectToList(ref list, prefab);
			}
			pooledGameobjects.Add(type, list);
		}
	}

	private void AddCreatedGameobjectToList(ref List<GameObject> list, GameObject prefab)
	{
		GameObject createdGameobject = Instantiate(prefab);
		createdGameobject.SetActive(false);
		list.Add(createdGameobject);
	}
}
