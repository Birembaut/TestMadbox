using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
	DamageAnimatorEvent damageAnimator;

	private void Awake()
	{
		damageAnimator = GetComponentInChildren<DamageAnimatorEvent>();
	}

	public void AnimationEnd()
	{
		transform.SetParent(null);
		GameManager.Instance.PoolManager.RecycleItem(gameObject, typeof(FloatingDamage));
	}

	public void TriggerAnimation(float Damage)
	{
		damageAnimator.StartAnimation(Damage);
	}
}
