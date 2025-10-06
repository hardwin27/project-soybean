using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private KeyCode reloadSceneKey;

    void Update()
    {
        if (Input.GetKeyDown(reloadSceneKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
