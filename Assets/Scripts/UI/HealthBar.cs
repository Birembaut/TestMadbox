using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Image TopBar;
	public Image BottomBar;
	public float Speed;
	Coroutine adjustCoroutine;

	private void Update()
	{
		transform.LookAt(Camera.main.transform.position);
	}

	public void RatioChanged(float ratio)
	{
		if(adjustCoroutine != null)
		{
			StopCoroutine(adjustCoroutine);
		}

		adjustCoroutine = StartCoroutine(AdjustBar(ratio));
	}

	public IEnumerator AdjustBar(float ratio)
	{
		TopBar.fillAmount = ratio;
		while (Mathf.Abs(TopBar.fillAmount - BottomBar.fillAmount) > Mathf.Epsilon)
		{
			BottomBar.fillAmount = Mathf.Lerp(BottomBar.fillAmount, ratio, Time.deltaTime * Speed);
			yield return null;
		}
		BottomBar.fillAmount = ratio;
	}
}
