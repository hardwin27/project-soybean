using System;
using UnityEngine;
using SingletonSystem;

public class AudioManager : Singleton<AudioManager>
{
    public SoundData[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private SFXManager sfxManager;

    private void Start()
    {
        sfxManager = SFXManager.Instance;
    }

    public void PlayMusic(string name)
    {
        SoundData s = Array.Find(musicSounds, x => x.Name == name);
        if (s == null)
        {
            Debug.Log("Music Not Found");
        }
        else
        {
            musicSource.clip = s.Clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        SoundData s = Array.Find(sfxSounds, x => x.Name == name);
        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else
        {
            sfxSource.clip = s.Clip;
            sfxSource.Play();
        }
    }

    public void PlaySFXObject(SoundData soundData)
    {
        sfxManager.PlaySFXClip(soundData.Clip, soundData.Volume);
    }
}
