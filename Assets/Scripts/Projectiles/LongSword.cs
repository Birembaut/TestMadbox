using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Projectile
{
	public override void RecycleProjectile()
	{
		GameManager.Instance.PoolManager.RecycleItem(gameObject, typeof(LongSword));
	}

	public override void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Enemy enemy = other.gameObject.GetComponent<Enemy>();
			enemy.IsTouched(Damage);
		}
	}
}
