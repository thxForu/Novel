using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour 
{
	public float volume, pitch;
	public AudioClip[] clips;
	public AudioClip[] music;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			AudioManager.instance.PlaySFX(clips[Random.Range(0,clips.Length)], volume, pitch);
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			AudioManager.instance.PlaySong(music[Random.Range(0, music.Length)]);
		}
	}
}
