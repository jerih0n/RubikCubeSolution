using RubikCube.Playground.Constants;
using RubikCube.Playground.Enums;

namespace RubikCube.Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var cube = new RubikCube();

            Console.WriteLine(cube.PrintMatrix());
        }
    }
}