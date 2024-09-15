using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public HealthBar HealthBar;
	public float BaseHealthMax = 10;
	public float Speed = 2;
	public float RoamChangeDirectionTimer = 3f;
	public float RoamDistance = 3f;
	private float healthMax;
	private float currentHealth;
	public EnemyState CurrentEnemyState;
	private Animation animation;
	private float ChangeTargetPositionDelay = 0;
	private Vector3 targetPosition = Vector3.zero;

	public enum EnemyState
	{
		Spawning,
		Roaming,
		Charging,
	}

	private void Awake()
	{
		animation = GetComponent<Animation>();
	}

	private void Update()
	{
		if(CurrentEnemyState == EnemyState.Spawning)
		{
			return;
		}

		Vector3 positionToReach = Vector3.zero;
		if(CurrentEnemyState == EnemyState.Charging)
		{
			positionToReach = GameManager.Instance.GetPlayerPosition();
		}
		else if(CurrentEnemyState == EnemyState.Roaming)
		{
			if(ChangeTargetPositionDelay <= 0)
			{
				ChangeTargetPositionDelay = RoamChangeDirectionTimer;
				targetPosition = transform.position + Vector3.forward * Random.Range(-RoamDistance, RoamDistance) + Vector3.right * Random.Range(-RoamDistance, RoamDistance);
			}

			positionToReach = targetPosition;
			ChangeTargetPositionDelay -= Time.deltaTime;
		}

		transform.position = Vector3.MoveTowards(transform.position, positionToReach, Speed * Time.deltaTime);
		transform.LookAt(positionToReach);
	}

	private void OnDeath()
	{
		GameManager.Instance.WaveManager.EnemyDied.Invoke(gameObject);
	}

	public void Reset(int wave)
	{
		healthMax = BaseHealthMax * (1 + wave * 0.2f);
		currentHealth = healthMax;
		HealthBar.RatioChanged(1);
		CurrentEnemyState = EnemyState.Spawning;
		animation.Play();
	}

	public void IsTouched(int damage)
	{
		if (CurrentEnemyState == EnemyState.Spawning)
		{
			return;
		}

		currentHealth -= damage;
		HealthBar.RatioChanged(currentHealth / healthMax);
		GameObject floatingDamage = GameManager.Instance.PoolManager.GetInstanciedPrefab(GameManager.Instance.FloatingDamagePrefab, transform.position, typeof(FloatingDamage));
		floatingDamage.GetComponent<FloatingDamage>().TriggerAnimation(damage);

		if (currentHealth <= 0)
		{
			OnDeath();
		}
		else
		{
			floatingDamage.transform.SetParent(transform, false);
			floatingDamage.transform.localPosition = Vector3.zero;
		}
	}

	public void SpawnEnd()
	{
		ChangeState();
	}

	private void ChangeState()
	{
		CurrentEnemyState = Random.value < 0.5f ? EnemyState.Roaming : EnemyState.Charging;
	}
}
