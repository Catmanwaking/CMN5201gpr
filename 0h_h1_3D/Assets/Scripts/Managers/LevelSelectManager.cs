using Optimized_0h_h1_3D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;

    public void SelecLevel(int size)
    {
        LevelCreator levelCreator = new();
        GameGrid grid = levelCreator.CreateLevel(size, 2);
        level.grid = new PlayerGrid(grid.Size, Rules.ColorCount); //TODO TMP TEST LINES THIS IS A MESS
        level.grid.SetFromGrid(grid);

        SceneManager.LoadScene(3);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
