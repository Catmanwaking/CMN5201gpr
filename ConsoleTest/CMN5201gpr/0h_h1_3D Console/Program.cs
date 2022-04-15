//Author: Dominik Dohmeier
namespace _0h_h1_3D_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BruteForceSolver solver = new BruteForceSolver();
            int count = solver.CountSolutions(8);

            System.Console.WriteLine(count);
            _ = System.Console.ReadKey();
        }
    }
}