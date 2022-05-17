//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void BackToLevelSelect()
    {
        SceneManager.LoadScene(2);
    }
}