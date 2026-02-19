using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string splashScreenVidPath;
    [SerializeField] private VideoPlayer customSplashPlayer;
    [SerializeField] private GameObject customSplashImage;
    [SerializeField] private GameObject webPlatformPanel;
    [SerializeField] GameObject mainMenuObj;

    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button quitGameBtn;
    [SerializeField] private Button webPlaytBtn;

    public void Awake()
    {
        mainMenuObj.SetActive(false);
        webPlatformPanel.SetActive(false);
        customSplashImage.SetActive(true);
        customSplashPlayer.loopPointReached += MenuAppear;
        startGameBtn.onClick.AddListener(StartGame);
        quitGameBtn.onClick.AddListener(QuitGame);

        customSplashPlayer.source = VideoSource.Url;
        customSplashPlayer.url = $"{Application.streamingAssetsPath + splashScreenVidPath}";
    }

    private void Start()
    {
/*#if UNITY_WEBGL
        webPlaytBtn.onClick.AddListener(() => {
            webPlatformPanel.SetActive(false);
            StartCoroutine(PlaySplashScreen());
        });
        webPlatformPanel.SetActive(true);
#else
        StartCoroutine(PlaySplashScreen());
#endif*/

        StartCoroutine(PlaySplashScreen());
    }

    private IEnumerator PlaySplashScreen()
    {
        yield return new WaitForEndOfFrame();
        customSplashImage.SetActive(true);
        customSplashPlayer.Play();
    }

    private void MenuAppear(VideoPlayer var)
    {
        mainMenuObj.SetActive(true);
        customSplashImage.SetActive(false);
    }

    
    private void StartGame()
    {
        AudioManager.Instance?.PlaySFXObject("ui_start_button");
        SceneManager.LoadSceneAsync("VerticalSlice");
    }

    private void QuitGame()
    {
        AudioManager.Instance?.PlaySFXObject("ui_quit_button");
        Application.Quit();
    }
}
