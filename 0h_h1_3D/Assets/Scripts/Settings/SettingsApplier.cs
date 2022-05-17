//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsApplier : MonoBehaviour
{
    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject StopwatchObject;
    [SerializeField] private GameObject swipeControlManager;

    public void ApplySettings()
    {
        Settings settings = SettingsLoader.LoadSettings();

        StopwatchObject.SetActive(settings.UseStopwatch == 1);
        hintButton.SetActive(settings.ShowHint == 1);

        ColorIndex.SetColorTheme(settings.ColorTheme);
    }

    private void Start()
    {
        ApplySettings();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += CheckOptionsClosed;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded += CheckOptionsClosed;
    }

    public void OpenOptions()
    {
        swipeControlManager.SetActive(false);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void CheckOptionsClosed(Scene scene)
    {
        if (scene.buildIndex == 1)
        {
            swipeControlManager.SetActive(true);
            ApplySettings();
        }
    }
}