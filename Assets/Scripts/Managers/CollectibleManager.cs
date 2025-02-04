using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class CollectibleManager : MonoBehaviour
{
	public Collectible[] collectibles;
	public float SpawnRate = 0.1f;

	private List<Collectible> collectibleList = new List<Collectible>();

	private void Start()
	{
		GameManager.Instance.CollectibleCollected += OnCollectCollectible;
		GameManager.Instance.PlayerDied += OnPlayerDied;
		GameManager.Instance.WaveManager.EnemyDied += OnEnemyDied;
	}

	private void OnDestroy()
	{
		for(int index = collectibleList.Count - 1; index >= 0; index--)
		{
			Destroy(collectibleList[index]);
		}

		GameManager.Instance.CollectibleCollected -= OnCollectCollectible;
		GameManager.Instance.PlayerDied -= OnPlayerDied;
		GameManager.Instance.WaveManager.EnemyDied -= OnEnemyDied;
	}

	private void OnEnemyDied(GameObject enemy, bool isKilled)
	{
		if(!isKilled)
		{
			return;
		}

		if(Random.value < SpawnRate)
		{
			collectibleList.Add(Instantiate(collectibles[Random.Range(0, collectibles.Length)], enemy.transform.position, Quaternion.identity));
		}
	}

	private void OnCollectCollectible(Collectible collectible)
	{
		collectibleList.Remove(collectible);
		Destroy(collectible.gameObject);
	}

	private void OnPlayerDied(int score)
	{
		for (int collectibleIndex = collectibleList.Count - 1; collectibleIndex >= 0; collectibleIndex--)
		{
			OnCollectCollectible(collectibleList[collectibleIndex]);
		}
	}
}
