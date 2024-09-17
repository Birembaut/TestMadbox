using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamagePSRecycler : MonoBehaviour
{
	public float Duration;

	private void Start()
	{
		Invoke("Recycle", Duration);
	}
	private void Recycle()
	{
		GameManager.Instance.PoolManager.RecycleItem(gameObject, typeof(DamagePSRecycler));
	}
}
