using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MenuFader
{
    private void Start()
    {
        FadeIn();
        //GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene((int)SceneIndex.Settings, LoadSceneMode.Additive);
        FadeOut();
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene((int)SceneIndex.LevelSelect, LoadSceneMode.Single);
    }

    public void PlayTutorial()
    {
        //SceneManager.LoadScene((int)SceneIndex.Tutorial, LoadSceneMode.Single);
    }

    public void OpenAbout()
    {
        SceneManager.LoadScene((int)SceneIndex.About, LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += CheckOptionsClosed;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= CheckOptionsClosed;
    }

    private void CheckOptionsClosed(Scene scene)
    {
        if (scene.buildIndex == (int)SceneIndex.Settings)
            FadeIn();
    }
}

public enum SceneIndex
{
    SplashScreen,
    MainMenu,
    Settings,
    LevelSelect,
    Game,
    Tutorial,
    About
}