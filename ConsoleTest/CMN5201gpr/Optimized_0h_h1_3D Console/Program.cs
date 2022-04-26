// See https://aka.ms/new-console-template for more information
using Optimized_0h_h1_3D_Console;
using Optimized_0h_h1_3D_Console.Extensions;

int seed = 13;

LevelCreator creator = new();
Board level = creator.CreateLevel(2, seed);
ConsoleDrawer.PrintLevel(level);