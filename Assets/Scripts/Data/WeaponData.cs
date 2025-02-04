using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
	public GameObject PrefabWeapon;
	public Projectile PrefabProjectile;
	public Sprite WeaponSprite;

	public float WalkAnimationSpeed;
	public float AttackAnimationSpeed;
	public float HeroSpeed;
}