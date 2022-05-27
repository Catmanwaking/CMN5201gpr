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
        if (currentPage >= pages.Length - 1)
        {
            OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.MainMenu, LoadSceneMode.Single);
            FadeOut();
        }
        else
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
            FadeIn();
        }
    }

    public void OpenStorePage(string ID)
    {
        Application.OpenURL($"market://details?id={ID}");
    }
}
