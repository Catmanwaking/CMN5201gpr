using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OpenOptions()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}