using RubikCube.Playground.Constants;
using RubikCube.Playground.Enums;

namespace RubikCube.Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cube = new RubikCube();

            Console.WriteLine("Oringal matrix \n");
            Console.WriteLine(cube.PrintMatrix());
            Console.WriteLine("----------------------------------\n");

            Console.WriteLine("After Rotations\n");
            // Execute rotation sequence: F, R', U, B', L, D'
            cube.RotateFront(true);      // F - clockwise
            cube.RotateRight(false);     // R - counter-clockwise
            cube.RotateUpper(true);       // U - clockwise
            cube.RotateBottom(false);    // B - counter-clockwise
            cube.RotateLeft(true);       // L - clockwise
            cube.RotateDown(false);      // D - counter-clockwise

            // Print final result
            Console.WriteLine(cube.PrintMatrix());

            Console.WriteLine("----------------------------------------");
        }
    }
}