using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SFXController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public IObjectPool<GameObject> Pool { get; set; }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void PlaySFX(AudioClip audioClip, float volume)
    {
        _audioSource.clip = audioClip;
        _audioSource.volume = volume;
        float clipLength = audioClip.length;
        _audioSource.Play();
        StartCoroutine(DisableSFX(clipLength));
    }

    private IEnumerator DisableSFX(float delay)
    {
        yield return new WaitForSeconds(delay);
        Pool.Release(gameObject);
    }
}
