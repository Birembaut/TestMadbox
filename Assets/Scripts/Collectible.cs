using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	public Transform root;
	public float RotationSpeed;
	public WeaponData WeaponData;

	private void Update()
	{
		root.Rotate(Vector3.up * Time.deltaTime * RotationSpeed);
	}
}
