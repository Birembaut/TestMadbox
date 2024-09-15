using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Projectile
{
	public Transform visualRoot;
	public float RotationSpeed = 3;

	public override void RecycleProjectile()
	{
		GameManager.Instance.PoolManager.RecycleItem(gameObject, typeof(GreatSword));
	}

	public override void Update()
	{
		base.Update();

		visualRoot.Rotate(Vector3.up * Time.deltaTime * RotationSpeed);
	}
}
