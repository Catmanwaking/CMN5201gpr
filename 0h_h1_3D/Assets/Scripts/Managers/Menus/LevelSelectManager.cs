using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MenuFader
{
    [SerializeField] private LevelSO level;
    [SerializeField] private TMP_Text experimental_Text;

    private void Start()
    {
        FadeIn();
    }

    public void SelecLevel(int size)
    {
        experimental_Text.text = "(Loading)";
        StartCoroutine(WaitForUI(size));       
    }

    private IEnumerator WaitForUI(int size)
    {       
        yield return null;
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