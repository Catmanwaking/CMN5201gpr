//Author: Dominik Dohmeier
using _0h_h1_2D_Console;
using System.Text;

namespace Extensions;

internal static class ConsoleDrawer
{
    public static void PrintLevel(Board board)
    {
        int sideLength = board.SideLength;
        StringBuilder builder = new();
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
                builder.Append(board[x, y]);
            builder.AppendLine();
        }
        Console.WriteLine(builder.ToString());
    }
}
