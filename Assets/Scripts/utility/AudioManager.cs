using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	[Range(0f, 1f)]
	public float volume = 0.75f;
	
	public string playing;//{ get; private set; }
	public string next;// { get; private set; }
	
	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;


	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = volume;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	private void Start()
	{
		Play("Main");
	}

	private void FixedUpdate()
	{
		if (playing != null)
		{
			Sound s = Array.Find(sounds, item => item.name == playing);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			if (next != null && !s.source.isPlaying)
			{
				Play(next);
				next = null;
			}
		}
	}

	public void Play(string sound)
	{
//		Debug.Log("play: " + sound);
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("PLay - Sound: " + name + " not found!");
			return;
		}

		s.source.volume = volume;
		s.source.Play();
		playing = sound;
	}

	public void Stop(string sound)
	{
//		Debug.Log("stop: " + sound);
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Stop - Sound: " + name + " not found!");
			return;
		}
		
		s.source.Stop();
		playing = null;
		next = null;
	}

	public void PlayAfter(string sound)
	{
//		Debug.Log("next: " + sound);
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("After Sound: " + name + " not found!");
			return;
		}

		next = sound;
	}

}
