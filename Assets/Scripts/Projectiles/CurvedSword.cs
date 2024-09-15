using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedSword : Projectile
{
	public override void RecycleProjectile()
	{
		GameManager.Instance.PoolManager.RecycleItem(gameObject, typeof(CurvedSword));
	}
}
