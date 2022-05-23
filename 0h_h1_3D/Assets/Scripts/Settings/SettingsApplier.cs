//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsApplier : MenuFader
{
    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject swipeControlManager;
    [SerializeField] private StopwatchManager stopwatchManager;

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
}