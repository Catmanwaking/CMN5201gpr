//Author: Dominik Dohmeier
// See https://aka.ms/new-console-template for more information

using _0h_h1_2D_Console;
using Extensions;

int seed = 12;

LevelCreator creator = new LevelCreator();

Board board = creator.CreateLevel(2, seed);
ConsoleDrawer.PrintLevel(board);

//PrintLevel(board);
Console.WriteLine("Done");
_ = Console.ReadKey();