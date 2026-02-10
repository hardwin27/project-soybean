using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public VideoPlayer customsplash;
    public GameObject mainmenu;

    public void Awake()
    { 
        mainmenu.SetActive(true);
        customsplash.gameObject.SetActive(false);

        customsplash.loopPointReached += MenuAppear; 

    }

    public void MenuAppear(VideoPlayer var)
    {
        mainmenu.SetActive(true);
        customsplash.gameObject.SetActive(false);
    }

    
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("VerticalSlice");
    }

    public void QuitGame()
    { 
        Application.Quit();
    }
}
