using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MenuFader
{
    [SerializeField] private LevelSO level;

    private void Start()
    {
        FadeIn();
    }

    public void SelecLevel(int size)
    {
        level.grid = new(size);

        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.Game);
        FadeOut();
    }

    public void BackToMainMenu()
    {
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.MainMenu);
        FadeOut();
    }
}
