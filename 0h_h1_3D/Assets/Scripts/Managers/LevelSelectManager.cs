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

        SceneManager.LoadScene((int)SceneIndex.Game);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene((int)SceneIndex.MainMenu);
    }
}
