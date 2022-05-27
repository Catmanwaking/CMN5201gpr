//Author: Dominik Dohmeier
// See https://aka.ms/new-console-template for more information
using Fast_0h_h1;
using System.Text;

LevelCreator creator = new(3);
int[,,] level = creator.GenerateLevel();
PrintLevel(level);

static void PrintLevel(int[,,] grid)
{
    int sideLength = grid.GetLength(0);
    StringBuilder builder = new();
    for (int x = 0; x < sideLength; x++)
    {
        for (int y = 0; y < sideLength; y++)
        {
            for (int z = 0; z < sideLength; z++)
                builder.Append(grid[x, y, z]);
            builder.Append(' ');
        }
        builder.AppendLine();
    }
    Console.WriteLine(builder.ToString());
}