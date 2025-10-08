using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Button restartGameButton;

    private void Start()
    {
        gameClearPanel.SetActive(false);
        questController.OnLastQuestCompleted += HandleLastQuestFinished;
        restartGameButton.onClick.AddListener(ReloadScene);
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
