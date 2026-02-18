using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
}
