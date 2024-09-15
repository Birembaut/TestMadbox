using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
	public Player Player;

	public void Attack()
	{
		Player.Attack();
	}

	public void FinishAttack()
	{
		Player.FinishAttack();
	}
}
