using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MenuFader
{
    [SerializeField] private LevelSO tmpLevelRef;

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
        //tmpLevelRef.grid = new  CubeGrid(3);
        //OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.Game, LoadSceneMode.Single);
        //FadeOut();
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