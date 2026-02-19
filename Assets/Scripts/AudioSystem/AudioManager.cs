using System;
using UnityEngine;
using SingletonSystem;

public class AudioManager : Singleton<AudioManager>
{
    public SoundData[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    [SerializeField] private string defaultUiClick;

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
            if (s.Clip != null)
            {
                musicSource.clip = s.Clip;
                musicSource.volume = s.Volume;
                musicSource.Play();
            }
            else
            {
                Debug.Log($"{s.Name} Music have no clip");
            }
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
            if (s.Clip != null)
            {
                sfxSource.clip = s.Clip;
                sfxSource.volume = s.Volume;
                sfxSource.Play();
            }
            else
            {
                Debug.Log($"{s.Name} SFX have no clip");
            }
        }
    }

    public void PlaySFXObject(string name)
    {
        SoundData s = Array.Find(sfxSounds, x => x.Name == name);
        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else
        {
            if (s.Clip != null)
            {
                sfxManager.PlaySFXClip(s.Clip, s.Volume);
            }
            else
            {
                Debug.Log($"{s.Name} SFX have no clip");
            }
        }
    }

    public void PlayDefaultUiClick()
    {
        PlaySFXObject(defaultUiClick);
    }
}
