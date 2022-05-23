using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutPageManager : MenuFader
{
    [SerializeField] private GameObject[] pages;
    private int currentPage;

    private void Start()
    {
        currentPage = 0;
        pages[0].SetActive(true);
        FadeIn();
    }

    public void CycleNextPage()
    {
        FadeOut();
        pages[currentPage].SetActive(false);
        currentPage++;
        if (currentPage >= pages.Length)
            SceneManager.LoadScene((int)SceneIndex.MainMenu, LoadSceneMode.Single);
        else
        {
            pages[currentPage].SetActive(true);
            FadeIn();
        }
    }

    public void OpenStorePage(string ID)
    {
        Application.OpenURL($"market://details?id={ID}");
    }
}
