using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Logic.Helpers
{
    public static class RotationHelpers
    {
        /// <summary>
        /// Rotates a 3x3 face clockwise in the matrix
        /// </summary>
        public static void RotateFaceClockwise(
            MatrixCellFillEnum[,] matrix,
            int startRow, int endRow,
            int startCol, int endCol)
        {
            // For a 3x3 grid, rotate clockwise by:
            // 1. Transpose (swap rows and columns)
            // 2. Reverse each row
            
            // Extract the 3x3 face
            var temp = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = matrix[startRow + i, startCol + j];
                }
            }

            // Rotate clockwise: transpose then reverse rows
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[startRow + i, startCol + j] = temp[2 - j, i];
                }
            }
        }

        /// <summary>
        /// Rotates a 3x3 face counter-clockwise in the matrix
        /// </summary>
        public static void RotateFaceCounterClockwise(
            MatrixCellFillEnum[,] matrix,
            int startRow, int endRow,
            int startCol, int endCol)
        {
            // For a 3x3 grid, rotate counter-clockwise by:
            // 1. Reverse each row
            // 2. Transpose (swap rows and columns)
            
            // Extract the 3x3 face
            var temp = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = matrix[startRow + i, startCol + j];
                }
            }

            // Rotate counter-clockwise: reverse rows then transpose
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[startRow + i, startCol + j] = temp[j, 2 - i];
                }
            }
        }

        /// <summary>
        /// Rotates edges clockwise around a face
        /// Pattern: Top → Right → Bottom → Left → Top
        /// When rotating edges, we need to account for orientation changes:
        /// - Horizontal edges (Top/Bottom) rotate to vertical edges (Right/Left) and vice versa
        /// - This requires reversing the order when transitioning between horizontal and vertical
        /// </summary>
        public static void RotateEdgesClockwise(
            MatrixCellFillEnum[,] matrix,
            List<Cell> topEdge,
            List<Cell> rightEdge,
            List<Cell> bottomEdge,
            List<Cell> leftEdge)
        {
            // Store the values
            var topValues = new MatrixCellFillEnum[3];
            var rightValues = new MatrixCellFillEnum[3];
            var bottomValues = new MatrixCellFillEnum[3];
            var leftValues = new MatrixCellFillEnum[3];

            // Read current values
            for (int i = 0; i < 3; i++)
            {
                topValues[i] = matrix[topEdge[i].Row, topEdge[i].Col];
                rightValues[i] = matrix[rightEdge[i].Row, rightEdge[i].Col];
                bottomValues[i] = matrix[bottomEdge[i].Row, bottomEdge[i].Col];
                leftValues[i] = matrix[leftEdge[i].Row, leftEdge[i].Col];
            }

            // Rotate clockwise: Top → Right → Bottom → Left → Top
            // When moving from horizontal (Top/Bottom) to vertical (Right/Left), reverse order
            // When moving from vertical (Right/Left) to horizontal (Top/Bottom), reverse order
            for (int i = 0; i < 3; i++)
            {
                // Top (horizontal) → Right (vertical): reverse order
                matrix[rightEdge[2 - i].Row, rightEdge[2 - i].Col] = topValues[i];
                // Right (vertical) → Bottom (horizontal): reverse order
                matrix[bottomEdge[2 - i].Row, bottomEdge[2 - i].Col] = rightValues[i];
                // Bottom (horizontal) → Left (vertical): reverse order
                matrix[leftEdge[2 - i].Row, leftEdge[2 - i].Col] = bottomValues[i];
                // Left (vertical) → Top (horizontal): reverse order
                matrix[topEdge[2 - i].Row, topEdge[2 - i].Col] = leftValues[i];
            }
        }

        /// <summary>
        /// Rotates edges counter-clockwise around a face
        /// Pattern: Top → Left → Bottom → Right → Top
        /// When rotating edges, we need to account for orientation changes:
        /// - Horizontal edges (Top/Bottom) rotate to vertical edges (Right/Left) and vice versa
        /// - This requires reversing the order when transitioning between horizontal and vertical
        /// </summary>
        public static void RotateEdgesCounterClockwise(
            MatrixCellFillEnum[,] matrix,
            List<Cell> topEdge,
            List<Cell> rightEdge,
            List<Cell> bottomEdge,
            List<Cell> leftEdge)
        {
            // Store the values
            var topValues = new MatrixCellFillEnum[3];
            var rightValues = new MatrixCellFillEnum[3];
            var bottomValues = new MatrixCellFillEnum[3];
            var leftValues = new MatrixCellFillEnum[3];

            // Read current values
            for (int i = 0; i < 3; i++)
            {
                topValues[i] = matrix[topEdge[i].Row, topEdge[i].Col];
                rightValues[i] = matrix[rightEdge[i].Row, rightEdge[i].Col];
                bottomValues[i] = matrix[bottomEdge[i].Row, bottomEdge[i].Col];
                leftValues[i] = matrix[leftEdge[i].Row, leftEdge[i].Col];
            }

            // Rotate counter-clockwise: Top → Left → Bottom → Right → Top
            // When moving from horizontal (Top/Bottom) to vertical (Right/Left), reverse order
            // When moving from vertical (Right/Left) to horizontal (Top/Bottom), reverse order
            for (int i = 0; i < 3; i++)
            {
                // Top (horizontal) → Left (vertical): reverse order
                matrix[leftEdge[2 - i].Row, leftEdge[2 - i].Col] = topValues[i];
                // Left (vertical) → Bottom (horizontal): reverse order
                matrix[bottomEdge[2 - i].Row, bottomEdge[2 - i].Col] = leftValues[i];
                // Bottom (horizontal) → Right (vertical): reverse order
                matrix[rightEdge[2 - i].Row, rightEdge[2 - i].Col] = bottomValues[i];
                // Right (vertical) → Top (horizontal): reverse order
                matrix[topEdge[2 - i].Row, topEdge[2 - i].Col] = rightValues[i];
            }
        }
    }
}
