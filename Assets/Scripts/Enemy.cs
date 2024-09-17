using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
	public AudioClip ExplosionSound;
	public AudioClip EnemyHitSound;
	public ExplosionPSRecycler ExplosionPSRecycler;
	public SkinnedMeshRenderer SkinnedMeshRenderer;
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
	private Coroutine flickColorCoroutine;


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

	private void OnDeath(bool isKilled)
	{
		GameManager.Instance.WaveManager.EnemyDied.Invoke(gameObject, isKilled);
	}

	public void Reset(int wave)
	{
		SkinnedMeshRenderer.material.SetColor("_BaseColor", Color.white);
		healthMax = BaseHealthMax * (1 + wave * 0.2f);
		currentHealth = healthMax;
		HealthBar.RatioChanged(1);
		CurrentEnemyState = EnemyState.Spawning;
		animation.Play();

		transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
	}

	public void IsTouched(int damage)
	{
		if (CurrentEnemyState == EnemyState.Spawning)
		{
			return;
		}

		if(CurrentEnemyState == EnemyState.Roaming)
		{
			CurrentEnemyState = EnemyState.Charging;
		}

		GameManager.Instance.AudioManager.AddSound(transform.position, EnemyHitSound);
		GameManager.Instance.DisplayDamageParticuleSystem(transform.position);
		currentHealth -= damage;
		HealthBar.RatioChanged(currentHealth / healthMax);
		GameObject floatingDamage = GameManager.Instance.PoolManager.GetInstanciedPrefab(GameManager.Instance.FloatingDamagePrefab, transform.position, typeof(FloatingDamage));
		floatingDamage.GetComponent<FloatingDamage>().TriggerAnimation(damage);

		if (currentHealth <= 0)
		{
			OnDeath(true);
		}
		else
		{
			if(flickColorCoroutine != null)
			{
				StopCoroutine(flickColorCoroutine);
			}
			flickColorCoroutine = StartCoroutine(FlickColor());
		}
	}

	public void SpawnEnd()
	{
		ChangeState();
	}

	private void ChangeState()
	{
		CurrentEnemyState = Random.value < 0.8f ? EnemyState.Roaming : EnemyState.Charging;
	}

	private IEnumerator FlickColor()
	{
		SkinnedMeshRenderer.material.SetColor("_BaseColor", Color.red);
		yield return new WaitForSeconds(0.1f);
		SkinnedMeshRenderer.material.SetColor("_BaseColor", Color.white);
	}

	private void OnTriggerStay(Collider other)
	{
		if(CurrentEnemyState == EnemyState.Spawning)
		{
			return;
		}

		if(other.tag == "Player")
		{
			GameManager.Instance.AudioManager.AddSound(transform.position, ExplosionSound);
			GameManager.Instance.PoolManager.GetInstanciedPrefab(ExplosionPSRecycler.gameObject, transform.position + Vector3.up / 2, typeof(ExplosionPSRecycler));
			OnDeath(false);
			Player player = other.GetComponent<Player>();
			player.OnHit();
		}
	}
}
