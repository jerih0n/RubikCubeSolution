using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Helpers;
using RubikCubeSolution.Logic.Models;

namespace RubikCube.Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("RubikCube rotation smoke checks...");

            SmokeCheck_AllMoves_InverseAndFourTurns();

            Console.WriteLine("OK");
        }

        private static void SmokeCheck_AllMoves_InverseAndFourTurns()
        {
            var faces = new[]
            {
                RubikCubeSideEnum.Front,
                RubikCubeSideEnum.Right,
                RubikCubeSideEnum.Upper,
                RubikCubeSideEnum.Left,
                RubikCubeSideEnum.Down,
                RubikCubeSideEnum.Bottom, // Back in this net
            };

            foreach (var face in faces)
            {
                var original = CreateUniqueIntMatrix(9, 12);

                // move + inverse restores
                var m1 = (int[,])original.Clone();
                RotationHelpers.RotateLayer(m1, face, true);
                RotationHelpers.RotateLayer(m1, face, false);
                AssertMatrixEqual(original, m1, $"({face}) cw then ccw should restore");

                // 4 turns restores
                var m2 = (int[,])original.Clone();
                for (int i = 0; i < 4; i++)
                    RotationHelpers.RotateLayer(m2, face, true);
                AssertMatrixEqual(original, m2, $"({face}) cw x4 should restore");
            }
        }

        private static int[,] CreateUniqueIntMatrix(int rows, int cols)
        {
            var m = new int[rows, cols];
            var v = 1;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    m[r, c] = v++;
                }
            }
            return m;
        }

        private static void AssertMatrixEqual(int[,] a, int[,] b, string message)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new Exception($"Matrix size mismatch: {message}");

            for (int r = 0; r < a.GetLength(0); r++)
            {
                for (int c = 0; c < a.GetLength(1); c++)
                {
                    if (a[r, c] != b[r, c])
                        throw new Exception($"Matrix mismatch at ({r},{c}): {message}");
                }
            }
        }
    }
}