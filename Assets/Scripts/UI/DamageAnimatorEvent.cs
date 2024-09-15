using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageAnimatorEvent : MonoBehaviour
{
	private FloatingDamage parentScript;
	private TextMeshPro textMeshPro;
	private Animation animation;

	private void Awake()
	{
		parentScript = transform.parent.GetComponent<FloatingDamage>();
		textMeshPro = GetComponent<TextMeshPro>();
		animation = GetComponent<Animation>();
	}

	private void Update()
	{
		transform.LookAt(Camera.main.transform.position);
	}

	public void OnAnimationEnd()
	{
		parentScript.AnimationEnd();
	}

	public void StartAnimation(float Damage)
	{
		textMeshPro.text = Damage.ToString();
		animation.Play();
	}
}
