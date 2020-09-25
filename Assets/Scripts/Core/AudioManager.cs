using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager instance;

	public static SONG activeSong = null;
	public static List<SONG> allSongs = new List<SONG>();

	public float songTransitionSpeed = 2f;
	public bool songSmoothTransitions = true;

	//Ambiance controller
	public static List<AudioSource> activeAmbiance = new List<AudioSource>();
	public static List<AudioSource> deactivatedAmbiance = new List<AudioSource>();

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			transform.parent = null;
			DontDestroyOnLoad(this);
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

    /// <summary>
    /// Returns a list of all the clips being played as ambiance right now.
    /// </summary>
    public static List<AudioClip> activeAmbianceClips
    {
        get
        {
            List<AudioClip> retVal = new List<AudioClip>();
            foreach (AudioSource s in activeAmbiance)
            {
                retVal.Add(s.clip);
            }
            return retVal;
        }
    }

    GameObject volatileParent = null;
    void MakeVolatileChild(Transform ob)
    {
        if (volatileParent == null)
            volatileParent = new GameObject("[AUDIOMANAGER VOLATILE]");

        ob.parent = volatileParent.transform;
    }

    /// <summary>
    /// Start playing an ambiance track.
    /// </summary>
    /// <param name="clip"></param>
    public void PlayAmbiance(AudioClip clip)
    {
        //only play the ambiance, if this clip is not already playing.
        for (int i = activeAmbiance.Count - 1; i >= 0; i--)
        {
            if (activeAmbiance[i] != null)
            {
                if (activeAmbiance[i].clip == clip)
                    return;
            }
            else
            {
                activeAmbiance.RemoveAt(i);
            }
        }

        //the clip is not playing. make it play
        AudioSource source = CreateNewSource(string.Format("AMBIANCE [{0}]", clip.name));
        MakeVolatileChild(source.transform);
        source.clip = clip;
        source.volume = 0;//start at zero so ambiance fades in.
        source.Play();
        activeAmbiance.Add(source);

        //make all ambiance fade in and out
        if (ambianceLeveling == null)
            ambianceLeveling = StartCoroutine(AmbianceLeveling());
    }

    /// <summary>
    /// Stop an ambiance clip.
    /// </summary>
    /// <param name="clip"></param>
    public void StopAmbiance(AudioClip clip)
    {
        StopAmbiance(clip.name);
    }

    /// <summary>
    /// Stop an ambiance clip by this name.
    /// </summary>
    /// <param name="clipName"></param>
    public void StopAmbiance(string clipName)
    {
        for (int i = activeAmbiance.Count - 1; i >= 0; i--)
        {
            if (activeAmbiance[i] == null)
            {
                activeAmbiance.RemoveAt(i);
                continue;
            }

            AudioSource s = activeAmbiance[i];
            if (s.clip.name.ToLower() == clipName.ToLower())
            {
                activeAmbiance.Remove(s);
                deactivatedAmbiance.Add(s);
            }
        }

        //make all ambiance fade in and out
        if (ambianceLeveling == null)
            ambianceLeveling = StartCoroutine(AmbianceLeveling());
    }

    /// <summary>
    /// Stop all ambiance clips playing on this manager.
    /// </summary>
    public void StopAmbiance()
    {
        for (int i = activeAmbiance.Count - 1; i >= 0; i--)
        {
            if (activeAmbiance[i] == null)
            {
                activeAmbiance.RemoveAt(i);
                continue;
            }

            AudioSource s = activeAmbiance[i];
            activeAmbiance.Remove(s);
            deactivatedAmbiance.Add(s);
        }

        //make all ambiance fade in and out
        if (ambianceLeveling == null)
            ambianceLeveling = StartCoroutine(AmbianceLeveling());
    }

    Coroutine ambianceLeveling = null;
    IEnumerator AmbianceLeveling()
    {
        bool handle = true;
        while (handle)
        {
            handle = false;
            //make all active ambiance transition in
            for (int i = 0; i < activeAmbiance.Count; i++)
            {
                AudioSource ambiance = activeAmbiance[i];
                if (ambiance.volume < 1)
                {
                    handle = true;
                    ambiance.volume = Mathf.MoveTowards(ambiance.volume, 1, 2f * Time.deltaTime);
                }
            }
            //silence and destroy all inactive ambiance.
            if (deactivatedAmbiance.Count > 0)
            {
                for (int i = deactivatedAmbiance.Count - 1; i >= 0; i--)
                {
                    AudioSource ambiance = deactivatedAmbiance[i];
                    if (ambiance.volume > 0)
                    {
                        handle = true;
                        ambiance.volume = Mathf.MoveTowards(ambiance.volume, 0, 2f * Time.deltaTime);
                        if (ambiance.volume == 0)
                        {
                            deactivatedAmbiance.Remove(ambiance);
                            Destroy(ambiance.gameObject, 0.01f);
                        }
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }

        ambianceLeveling = null;
    }
    public void PlaySFX(AudioClip effect, float volume = 1f, float pitch = 1f)
	{
		AudioSource source = CreateNewSource(string.Format("SFX [{0}]", effect.name));
		source.clip = effect;
		source.volume = volume;
		source.pitch = pitch;
		source.Play();

		Destroy(source.gameObject, effect.length);
	}

	public void PlaySong(AudioClip song, float maxVolume = 1f, float pitch = 1f, float startingVolume = 0f, bool playOnStart = true, bool loop = true)
	{
		if (song != null)
		{
			for(int i = 0; i < allSongs.Count; i++)
			{
				SONG s = allSongs[i];
				if (s.clip == song)
				{
					activeSong = s;
					break;
				}
			}
			if (activeSong == null || activeSong.clip != song)
				activeSong = new SONG(song, maxVolume, pitch, startingVolume, playOnStart, loop);
		}
		else 
			activeSong = null;

		StopAllCoroutines();
		StartCoroutine(VolumeLeveling());
	}

	IEnumerator VolumeLeveling()
	{
		while(TransitionSongs())
			yield return new WaitForEndOfFrame();
	}

	bool TransitionSongs()
	{
		bool anyValueChanged = false;

		float speed = songTransitionSpeed * Time.deltaTime;
		for (int i = allSongs.Count - 1; i >= 0; i--) 
		{
			SONG song = allSongs [i];
			if (song == activeSong) 
			{
				if (song.volume < song.maxVolume) 
				{
					song.volume = songSmoothTransitions ? Mathf.Lerp (song.volume, song.maxVolume, speed) : Mathf.MoveTowards (song.volume, song.maxVolume, speed);
					anyValueChanged = true;
				}
			} 
			else 
			{
				if (song.volume > 0) 
				{
					song.volume = songSmoothTransitions ? Mathf.Lerp (song.volume, 0f, speed) : Mathf.MoveTowards (song.volume, 0f, speed);
					anyValueChanged = true;
				}
				else
				{
					allSongs.RemoveAt (i);
					song.DestroySong();
					continue;
				}
			}
		}

		return anyValueChanged;
	}

	public static AudioSource CreateNewSource(string _name)
	{
		AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
		newSource.transform.SetParent(instance.transform);
		return newSource;
	}

	[System.Serializable]
	public class SONG
	{
		public AudioSource source;
		public AudioClip clip {get{return source.clip;} set{source.clip = value;}}
		public float maxVolume = 1f;

		public SONG(AudioClip clip, float _maxVolume, float pitch, float startingVolume, bool playOnStart, bool loop)
		{
			source = AudioManager.CreateNewSource(string.Format("SONG [{0}]", clip.name));
			source.clip = clip;
			source.volume = startingVolume;
			maxVolume = _maxVolume;
			source.pitch = pitch;
			source.loop = loop;

			AudioManager.allSongs.Add(this);

			if (playOnStart)
				source.Play();
		}

		public float volume { get{ return source.volume;} set{source.volume = value;}}
		public float pitch {get{return source.pitch;} set{source.pitch = value;}}

		public void Play()
		{
			source.Play();
		}

		public void Stop()
		{
			source.Stop();
		}

		public void Pause()
		{
			source.Pause();
		}

		public void UnPause()
		{
			source.UnPause();
		}

		public void DestroySong()
		{
			AudioManager.allSongs.Remove(this);
			DestroyImmediate(source.gameObject);
		}
	}
}
