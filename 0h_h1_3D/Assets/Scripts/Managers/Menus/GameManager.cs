//Author: Dominik Dohmeier
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MenuFader
{
    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject swipeControlManager;
    [SerializeField] private StopwatchManager stopwatchManager;
    [SerializeField] private GameObject[] DisableOnGameEnd;

    public void ApplySettings()
    {
        Settings settings = SettingsLoader.LoadSettings();

        stopwatchManager.gameObject.SetActive(settings.UseStopwatch == 1);
        hintButton.SetActive(settings.ShowHint == 1);

        ColorIndex.SetColorTheme(settings.ColorTheme);
    }

    private void Start()
    {
        ApplySettings();
        FadeIn();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += CheckOptionsClosed;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= CheckOptionsClosed;
    }

    public void OpenOptions()
    {
        FadeOut();
        swipeControlManager.SetActive(false);
        stopwatchManager.TimerActive = false;
        SceneManager.LoadScene((int)SceneIndex.Settings, LoadSceneMode.Additive);
    }

    private void CheckOptionsClosed(Scene scene)
    {
        if (scene.buildIndex == (int)SceneIndex.Settings)
        {
            swipeControlManager.SetActive(true);
            stopwatchManager.TimerActive = true;
            ApplySettings();
            FadeIn();
        }
    }

    public void ReturnToLevelSelect(bool immediate)        
    {
        if (immediate)
            ReturnToLevelSelect();
        else           
            StartCoroutine(ReturnDelayed());
    }

    private IEnumerator ReturnDelayed()
    {
        foreach (GameObject item in DisableOnGameEnd)
            item.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        ReturnToLevelSelect();
    }

    private void ReturnToLevelSelect()
    {
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.LevelSelect);
        FadeOut();
    }
}