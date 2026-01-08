using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Button restartGameButton;

    private AudioManager audioManager;
    private GameTimeManager gameTimeManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
        gameTimeManager = GameTimeManager.Instance;
    }

    private void Start()
    {
        gameClearPanel.SetActive(false);
        questController.OnLastQuestCompleted += HandleLastQuestFinished;
        restartGameButton.onClick.AddListener(ReloadScene);

        gameTimeManager.StartNextDay();

        audioManager.PlayMusic("main_bgm");
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
