using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
	public Transform WeaponSlot;

	private CharacterController characterController;
	private Animator animator;

	private float speed;
	private Vector3 direction;

	private Enemy target;
	private WeaponData currentWeapon;

	private bool CanAttack = true;

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
		GetComponentInChildren<PlayerAnimatorEvent>().Player = this;

		ChooseStartEquipment();

		GameManager.Instance.InputManager.PlayerMoved += OnPlayerMoved;
		GameManager.Instance.WaveManager.EnemyDied += OnEnemyDied;
	}

	private void OnDestroy()
	{
		GameManager.Instance.InputManager.PlayerMoved -= OnPlayerMoved;
		GameManager.Instance.WaveManager.EnemyDied -= OnEnemyDied;
	}

	private void Update()
	{
		characterController.Move(direction * speed * Time.deltaTime);
		animator.SetFloat("Speed", direction.magnitude);

		if (target == null && direction == Vector3.zero)
		{
			target = GameManager.Instance.WaveManager.FindNearestTarget(transform.position);
		}

		if (target != null)
		{
			transform.LookAt(target.transform.position);

			if (CanAttack)
			{
				CanAttack = false;
				animator.SetTrigger("Attack");
			}
		}

#if UNITY_EDITOR
		// Cheat code to change weapons
		if(Input.GetKeyDown(KeyCode.F1))
		{
			EquipEquipment(GameManager.Instance.WeaponDatas[0]);
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			EquipEquipment(GameManager.Instance.WeaponDatas[1]);
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			EquipEquipment(GameManager.Instance.WeaponDatas[2]);
		}
#endif
	}

	private void OnPlayerMoved(Vector2 movement)
	{
		target = null;
		direction = new Vector3(movement.x, 0, movement.y);
		if (movement != Vector2.zero)
		{
			transform.rotation = Quaternion.LookRotation(direction);
			CanAttack = true;
			WeaponSlot.gameObject.SetActive(true);
		}
	}

	private void OnEnemyDied(GameObject enemy)
	{
		if (enemy == target.gameObject)
		{
			target = null;
		}
	}

	private void EquipEquipment(WeaponData weapon)
	{
		currentWeapon = weapon;
		GameObject weaponInstance = Instantiate(currentWeapon.PrefabWeapon, WeaponSlot);
		speed = currentWeapon.HeroSpeed;
		animator.SetFloat("AttackSpeed", currentWeapon.AnimationSpeed);
	}

	private void ChooseStartEquipment()
	{
		int weaponCount = GameManager.Instance.WeaponDatas.Length;
		WeaponData choosenWeapon = GameManager.Instance.WeaponDatas[Random.Range(0, weaponCount)];
		EquipEquipment(choosenWeapon);
	}

	public void Attack()
	{
		WeaponSlot.gameObject.SetActive(false);
		GameObject projectileGameobject = GameManager.Instance.PoolManager.GetInstanciedPrefab(currentWeapon.PrefabProjectile.gameObject, transform.position, currentWeapon.PrefabProjectile.GetType());
		Projectile projectile = projectileGameobject.GetComponent<Projectile>();
		projectileGameobject.transform.rotation = transform.rotation;
		projectile.Reset();
	}

	public void FinishAttack()
	{
		WeaponSlot.gameObject.SetActive(true);
		CanAttack = true;
	}
}
