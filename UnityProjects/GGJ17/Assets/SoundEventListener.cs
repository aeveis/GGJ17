using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEventListener : MonoBehaviour
{
	public AudioSource SourceOverride;
	public List<AudioClip> randomGroup = new List<AudioClip> ();
	public bool PreventRepeats = false;

	int previousPlayed = -1;

	public void Play(AudioClip clip)
	{
		if (!SourceOverride)
			SoundManager.instance.playSingle (clip);
		else
			SourceOverride.PlayOneShot (clip);
	}

	public void PlayRandomFromGroup(float pitchVariance)
	{
		if (randomGroup.Count == 0)
			return;
		
		int id = Random.Range (0, randomGroup.Count - 1);

		if (id == previousPlayed && PreventRepeats == true) {
			if (randomGroup.Count == id + 1 || randomGroup.Count == 1)
				id = 0;
			else
				id++;
		}
		if (!SourceOverride) {
			if (pitchVariance == 0)
				SoundManager.instance.playSingle (randomGroup [id]);
			else
				SoundManager.instance.playSingle (randomGroup [id], pitchVariance);
		} else {
			SourceOverride.pitch = Random.Range (1f - pitchVariance, 1f + pitchVariance);
			SourceOverride.PlayOneShot (randomGroup [id]);
		}
	}
}
