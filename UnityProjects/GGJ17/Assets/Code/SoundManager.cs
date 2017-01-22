using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource efxSource;
	public AudioSource pitchedSource;
	public AudioSource pitchedVolumeSource;
	public static SoundManager instance = null;

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);

	}

	public void playSingle(AudioClip clip)
	{
		efxSource.PlayOneShot (clip);
	}

	public void playSingle(AudioClip clip, float pitchVariance)
	{
		float pitch = Random.Range (1f - pitchVariance, 1f + pitchVariance);
		pitchedSource.pitch = pitch;
		pitchedSource.PlayOneShot (clip);
	}

	public void playSingle(AudioClip clip, float pitchVariance, float volume)
	{
		float pitch = Random.Range (1f - pitchVariance, 1f + pitchVariance);
		pitchedVolumeSource.pitch = pitch;
		pitchedVolumeSource.volume = volume;
		pitchedVolumeSource.PlayOneShot (clip);
	}

}
