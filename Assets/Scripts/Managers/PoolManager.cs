using UnityEngine;
using System.Collections.Generic;
using System;

public class PoolManager : MonoBehaviour
{
	private Dictionary<Type, List<GameObject>> pooledGameobjects = new Dictionary<Type, List<GameObject>> ();
	private Dictionary<Type, GameObject> rootGameobjects = new Dictionary<Type, GameObject>();

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
		List<GameObject> list = pooledGameobjects[type];
		list.Add(item);
		pooledGameobjects[type] = list;
		GameObject root = rootGameobjects[type];
		item.transform.SetParent(root.transform);
		item.gameObject.SetActive(false);
	}

	public void CreateOrFeedPool(GameObject prefab, int amount, Type type)
	{
		if(pooledGameobjects.ContainsKey(type))
		{
			List<GameObject> list = pooledGameobjects[type];
			GameObject root = rootGameobjects[type];

			for (int current = 0; current < amount; current++)
			{
				AddCreatedGameobjectToList(ref list, prefab, root.transform);
			}
			pooledGameobjects[type] = list;
		}
		else
		{
			GameObject rootType = new GameObject();
			rootType.transform.SetParent(transform);
			rootGameobjects.Add(type, rootType);
			rootType.name = type.ToString();

			List<GameObject> list = new List<GameObject>();
			for (int current = 0; current < amount; current++)
			{
				AddCreatedGameobjectToList(ref list, prefab, rootType.transform);
			}
			pooledGameobjects.Add(type, list);
		}
	}

	private void AddCreatedGameobjectToList(ref List<GameObject> list, GameObject prefab, Transform parent)
	{
		GameObject createdGameobject = Instantiate(prefab);
		createdGameobject.transform.SetParent(parent);
		createdGameobject.SetActive(false);
		list.Add(createdGameobject);
	}
}
