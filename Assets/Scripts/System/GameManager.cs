using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SingletonSystem;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private QuestController questController;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Button restartGameButton;

    private AudioManager audioManager;
    private GameTimeManager gameTimeManager;
    private ProgressionManager progressionManager;

    public Action OnDayStageStarted;
    public Action OnDayStageEnded;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
        gameTimeManager = GameTimeManager.Instance;
        progressionManager = ProgressionManager.Instance;

        gameTimeManager.OnDayEnded += HandleDayEnded;
    }

    private void Start()
    {
        gameClearPanel.SetActive(false);
        /*questController.OnLastQuestCompleted += HandleLastQuestFinished;
        restartGameButton.onClick.AddListener(ReloadScene);*/

        StartFirstDay();

        audioManager.PlayMusic("main_bgm");
    }

    private void StartFirstDay()
    {
        progressionManager.StartProgression();
        gameTimeManager.StartNextDay();
        OnDayStageStarted?.Invoke();
    }

    private void StartDayStage()
    {
        progressionManager.StartProgression();
        gameTimeManager.StartNextDay();
        OnDayStageStarted?.Invoke();
    }

    private void HandleDayEnded()
    {
        progressionManager.EndCurrentProgression();
        OnDayStageEnded?.Invoke();
    }

    private void HandleLastQuestFinished()
    {
        gameClearPanel.SetActive(true);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
