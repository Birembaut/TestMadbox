using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public delegate void OnPlayerMoved(Vector2 movement);
	public OnPlayerMoved PlayerMoved;
	private Vector2 movement;


	public void OnInputMoved(Vector2 input)
	{
		if (input != movement)
		{
			movement = input;
			PlayerMoved.Invoke(movement);
		}
	}
}
