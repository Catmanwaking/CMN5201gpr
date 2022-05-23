using UnityEngine.SceneManagement;

public class SplashScreenManager : MenuFader
{
    void Start()
    {
        FadeIn();
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.MainMenu);
    }

    public void LoadMainMenu()
    {
        FadeOut();
    }
}
