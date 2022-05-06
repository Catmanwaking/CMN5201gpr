// See https://aka.ms/new-console-template for more information
using Fast_0h_h1;
using Extensions;

int seed = 12;

LevelCreator creator = new(3);
int[,,] grid = creator.GenerateLevel(seed);
ConsoleDrawer.PrintLevel(grid);
