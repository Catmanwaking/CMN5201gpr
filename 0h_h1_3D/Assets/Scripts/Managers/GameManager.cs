//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject swipeControlMalager;

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
        swipeControlMalager.SetActive(false);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadScene(2);
    }

    private void CheckOptionsClosed(Scene scene)
    {
        if(scene.buildIndex == 1)
            swipeControlMalager.SetActive(true);
    }
}