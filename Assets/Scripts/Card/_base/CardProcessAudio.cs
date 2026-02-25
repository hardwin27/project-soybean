using UnityEngine;

[RequireComponent(typeof(CardProcessor))]
public class CardProcessAudio : MonoBehaviour
{
    private CardProcessor processor;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private string doingProcessAudioCode;
    [SerializeField] private string processDoneAudioCode;

    private AudioManager audioManager;

    private void Awake()
    {
        processor = GetComponent<CardProcessor>();

        audioManager = AudioManager.Instance;

        if (audioManager != null ) 
        {
            processor.OnProcessStarted += PlayProcessingAudio;
            processor.OnProcessFinished += PlayProcessDoneAudio;
        }
    }

    private void PlayProcessingAudio()
    {
        SoundData doingProcessSoundData = audioManager.GetSoundData(doingProcessAudioCode);
        if (doingProcessSoundData != null ) 
        {
            audioSource.loop = true;
            audioSource.clip = doingProcessSoundData.Clip;
            audioSource.volume = doingProcessSoundData.Volume;
            audioSource.Play();
        }
    }

    private void PlayProcessDoneAudio()
    {
        audioSource.Stop();
        audioManager.PlaySFXObject(processDoneAudioCode);
    }
}
