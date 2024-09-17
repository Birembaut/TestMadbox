using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public GameObject AudioSourcePrefab;

	public void AddSound(Vector3 position, AudioClip clip)
	{
		GameObject audioSourceGameplay = GameManager.Instance.PoolManager.GetInstanciedPrefab(AudioSourcePrefab, position, typeof(AudioSourceRecycler));
		AudioSource audioSource = audioSourceGameplay.GetComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.Play();
	}
}
