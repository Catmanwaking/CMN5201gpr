using UnityEngine.SceneManagement;

public class MainMenuManager : MenuFader
{
    private void Start()
    {
        FadeIn();
    }

    public void OpenOptions()
    {
        OnFadeOutComplete += LoadOptionsAdditive;
        FadeOut();
    }

    private void LoadOptionsAdditive()
    {
        SceneManager.LoadScene((int)SceneIndex.Settings, LoadSceneMode.Additive);
        OnFadeOutComplete -= LoadOptionsAdditive;
    }

    public void OpenLevelSelect()
    {
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.LevelSelect, LoadSceneMode.Single);
        FadeOut();
    }

    public void PlayTutorial()
    {
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.Tutorial, LoadSceneMode.Single);
        FadeOut();
    }

    public void OpenAbout()
    {
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.About, LoadSceneMode.Single);
        FadeOut();
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