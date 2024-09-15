using UnityEngine;

public class Enemy : MonoBehaviour
{
	public HealthBar HealthBar;
	public float BaseHealthMax = 10;
	private float healthMax;
	private float currentHealth;

	private void Start()
	{
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
	}

	public void IsTouched(int damage)
	{
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
}
