using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
	public Collectible[] collectibles;
	public float SpawnRate = 0.1f;

	private List<Collectible> collectibleList = new List<Collectible>();

	private void Start()
	{
		GameManager.Instance.WaveManager.EnemyDied += OnEnemyDied;
	}

	private void OnDestroy()
	{
		for(int index = collectibleList.Count - 1; index >= 0; index--)
		{
			Destroy(collectibleList[index]);
		}

		GameManager.Instance.WaveManager.EnemyDied -= OnEnemyDied;
	}

	private void OnEnemyDied(GameObject enemy)
	{
		if(Random.value < SpawnRate)
		{
			collectibleList.Add(Instantiate(collectibles[Random.Range(0, collectibles.Length)], enemy.transform.position, Quaternion.identity));
		}
	}
}
