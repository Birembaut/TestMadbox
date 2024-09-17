using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public delegate void OnEnemyDied(GameObject enemy, bool isKilled);
	public OnEnemyDied EnemyDied;
	public GameObject EnemiesPrefab;

	public float SpawnRadius = 5;

	private int waveCount = 0;
	private List<Enemy> instanciedEnemies = new List<Enemy>();

	private void Awake()
	{
		EnemyDied += OnEnemyDiedEvent;
	}

	private void OnDestroy()
	{
		EnemyDied -= OnEnemyDiedEvent;
	}

	private void Start()
	{
		InstanceWave(0);
	}

	private void InstanceWave(int waveCount)
	{
		int waveEnemies = waveCount * 2 + 2;
		for(int index = 0; index < waveEnemies; index++)
		{
			Vector3 position = GetEnemySpawnDirection(waveEnemies, index) * SpawnRadius;
			Quaternion rotation = Quaternion.identity;
			GameObject enemyGameobject = GameManager.Instance.PoolManager.GetInstanciedPrefab(EnemiesPrefab, position, typeof(Enemy));
			Enemy enemy = enemyGameobject.GetComponent<Enemy>();
			enemy.transform.LookAt(Vector3.zero);
			instanciedEnemies.Add(enemy);
			enemy.Reset(waveCount);
		}
	}

	private Vector3 GetEnemySpawnDirection(int numberMax, int currentIndex)
	{
		float radians = 2 * Mathf.PI / numberMax * currentIndex;

		float vertical = Mathf.Sin(radians);
		float horizontal = Mathf.Cos(radians);

		return new Vector3(horizontal, 0, vertical);
	}

	private void OnEnemyDiedEvent(GameObject enemy, bool isKilled)
	{
		Enemy enemyScript = enemy.GetComponent<Enemy>();
		GameManager.Instance.PoolManager.RecycleItem(enemy, typeof(Enemy));
		instanciedEnemies.Remove(enemyScript);

		if(instanciedEnemies.Count == 0)
		{
			waveCount++;
			InstanceWave(waveCount);
			Debug.Log($"Switch to Wave {waveCount}");
		}
	}

	public Enemy FindNearestTarget(Vector3 position)
	{
		Enemy nearestTarget = null;
		float distance = float.MaxValue;

		int enemiesCount = instanciedEnemies.Count;
		for (int enemyIndex = 0; enemyIndex < enemiesCount; enemyIndex++)
        {
            Enemy enemy = instanciedEnemies[enemyIndex];

			if(enemy.CurrentEnemyState == Enemy.EnemyState.Spawning)
			{
				continue;
			}

			float currentDistance = Vector3.Distance(position, enemy.transform.position);
			if(currentDistance < distance)
			{
				distance = currentDistance;
				nearestTarget = enemy;
			}
        }

		return nearestTarget;
    }
}
