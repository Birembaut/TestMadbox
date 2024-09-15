using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int HealthMax = 10;
	private int currentHealth;

	private void Start()
	{
	}

	private void OnDeath()
	{
		GameManager.Instance.WaveManager.EnemyDied.Invoke(gameObject);
	}

	public void Reset(int wave)
	{
		currentHealth = (int)(HealthMax * (1 + wave * 0.2f));
	}

	public void IsTouched(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			OnDeath();
		}
	}
}
