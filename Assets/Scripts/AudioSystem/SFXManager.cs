using UnityEngine;
using SingletonSystem;
using PoolingSystem;

public class SFXManager : Singleton<SFXManager>
{
    [SerializeField] private SFXController _sfxController;
    public void PlaySFXClip(AudioClip audioClip, float volume, Transform spawnTransform = null)
    {
        if (ObjectPoolController.Instance == null)
        {
            return;
        }

        GameObject audioObject = ObjectPoolController.Instance.Get(_sfxController.gameObject);
        if (spawnTransform != null)
        {
            audioObject.transform.position = spawnTransform.transform.position;
        }

        SFXController sfxController = audioObject.GetComponent<SFXController>();
        sfxController.PlaySFX(audioClip, volume);
    }
}
