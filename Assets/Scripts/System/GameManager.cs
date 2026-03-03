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
    private PopUpManager popUpManager;

    public Action OnDayStageStarted;
    public Action OnDayStageEnded;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
        gameTimeManager = GameTimeManager.Instance;
        progressionManager = ProgressionManager.Instance;
        popUpManager = PopUpManager.Instance;

        gameTimeManager.OnDayEnded += HandleDayEnded;
    }

    private void Start()
    {
        gameClearPanel.SetActive(false);
        /*questController.OnLastQuestCompleted += HandleLastQuestFinished;
        restartGameButton.onClick.AddListener(ReloadScene);*/

        audioManager.PlayMusic("main_bgm");

        popUpManager.OpenPopUp(
            "Welcome!", 
            "Check out the Quest Tab on the right to see your current objectives.", 
        StartFirstDay);
    }

    private void StartFirstDay()
    {
        progressionManager.StartProgression();
        gameTimeManager.StartNextDay();
        OnDayStageStarted?.Invoke();
    }

    public void StartNextDayStage()
    {
        if (gameTimeManager.CurrentDay < 7)
        {
            progressionManager.StartNextProgression();
            gameTimeManager.StartNextDay();
            OnDayStageStarted?.Invoke();
        }
        else
        {
            progressionManager.EndCurrentBatchProgression();
        }
    }

    public void StartNextWeekStage()
    {
        progressionManager.StartNextBatchProgression();
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
