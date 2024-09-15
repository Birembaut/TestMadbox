using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float Speed;
	public int Damage;
	public float Duration;
	private float currentDuration;

	public virtual void Update()
	{
		transform.position += transform.forward * Speed * Time.deltaTime;
		currentDuration += Time.deltaTime;
		if(currentDuration > Duration)
		{
			RecycleProjectile();
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			Enemy enemy = other.gameObject.GetComponent<Enemy>();
			enemy.IsTouched(Damage);
			RecycleProjectile();
		}
	}

	public virtual void RecycleProjectile()
	{
	}

	public void Reset()
	{
		currentDuration = 0;
	}
}
