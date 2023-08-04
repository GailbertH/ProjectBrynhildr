using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
	[SerializeField] AudioSource bgmClip;
	[SerializeField] AudioSource audioSources;

	private static SoundManager instance;
	public static SoundManager Instance { get { return instance; } }

	void Awake()
	{
		instance = this;
	}

	public void PlayBGM()
	{
		//bgmClip.Play (22050);
	}
}
