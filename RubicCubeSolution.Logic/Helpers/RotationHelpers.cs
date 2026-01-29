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
        /// Checks if an edge is horizontal (same row) or vertical (same column)
        /// </summary>
        private static bool IsHorizontalEdge(List<Cell> edge)
        {
            if (edge.Count < 2) return true;
            return edge[0].Row == edge[1].Row;
        }

        /// <summary>
        /// Rotates edges clockwise around a face
        /// Pattern: Top → Right → Bottom → Left → Top
        /// Reverse order when transitioning between horizontal and vertical edges
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

            // Determine orientations
            bool topIsHorizontal = IsHorizontalEdge(topEdge);
            bool rightIsHorizontal = IsHorizontalEdge(rightEdge);
            bool bottomIsHorizontal = IsHorizontalEdge(bottomEdge);
            bool leftIsHorizontal = IsHorizontalEdge(leftEdge);

            // Special case: Right and Left faces have all vertical edges
            // For these faces, we need to reverse the TARGET index instead
            bool allVertical = !topIsHorizontal && !rightIsHorizontal && !bottomIsHorizontal && !leftIsHorizontal;

            // Rotate clockwise: Top → Right → Bottom → Left → Top
            // When transitioning between horizontal and vertical, reverse the SOURCE index
            // Exception: For Right/Left faces (all vertical), reverse TARGET index instead
            // For Upper/Down faces (all horizontal), normal logic handles it (no reversal since same orientation)
            for (int i = 0; i < 3; i++)
            {
                if (allVertical)
                {
                    // Special case for Right/Left faces: reverse target index
                    matrix[rightEdge[2 - i].Row, rightEdge[2 - i].Col] = topValues[i];
                    matrix[bottomEdge[2 - i].Row, bottomEdge[2 - i].Col] = rightValues[i];
                    matrix[leftEdge[2 - i].Row, leftEdge[2 - i].Col] = bottomValues[i];
                    matrix[topEdge[2 - i].Row, topEdge[2 - i].Col] = leftValues[i];
                }
                else
                {
                    // Normal case: reverse source if different orientation
                    // For all-horizontal (Upper/Down) or all-vertical faces, orientations are same, so no reversal
                    int topSrcIdx = (topIsHorizontal != rightIsHorizontal) ? (2 - i) : i;
                    matrix[rightEdge[i].Row, rightEdge[i].Col] = topValues[topSrcIdx];
                    
                    int rightSrcIdx = (rightIsHorizontal != bottomIsHorizontal) ? (2 - i) : i;
                    matrix[bottomEdge[i].Row, bottomEdge[i].Col] = rightValues[rightSrcIdx];
                    
                    int bottomSrcIdx = (bottomIsHorizontal != leftIsHorizontal) ? (2 - i) : i;
                    matrix[leftEdge[i].Row, leftEdge[i].Col] = bottomValues[bottomSrcIdx];
                    
                    int leftSrcIdx = (leftIsHorizontal != topIsHorizontal) ? (2 - i) : i;
                    matrix[topEdge[i].Row, topEdge[i].Col] = leftValues[leftSrcIdx];
                }
            }
        }

        /// <summary>
        /// Rotates edges counter-clockwise around a face
        /// Pattern: Top → Left → Bottom → Right → Top
        /// Reverse order when transitioning between horizontal and vertical edges
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

            // Determine orientations
            bool topIsHorizontal = IsHorizontalEdge(topEdge);
            bool rightIsHorizontal = IsHorizontalEdge(rightEdge);
            bool bottomIsHorizontal = IsHorizontalEdge(bottomEdge);
            bool leftIsHorizontal = IsHorizontalEdge(leftEdge);

            // Special case: Right and Left faces have all vertical edges
            // For these faces, we need to reverse the TARGET index instead
            bool allVertical = !topIsHorizontal && !rightIsHorizontal && !bottomIsHorizontal && !leftIsHorizontal;
            bool allHorizontal = topIsHorizontal && rightIsHorizontal && bottomIsHorizontal && leftIsHorizontal;
            
            // Special case: Back face has horizontal-vertical-horizontal-vertical pattern
            // Detect Back face by checking if TopEdge is at row 0 AND BottomEdge is at row 8
            // Front face TopEdge is at row 2, BottomEdge is at row 6, so we can distinguish them
            bool isBackFacePattern = topIsHorizontal && !rightIsHorizontal && bottomIsHorizontal && !leftIsHorizontal 
                && topEdge.Count > 0 && topEdge[0].Row == 0 
                && bottomEdge.Count > 0 && bottomEdge[0].Row == 8;

            // Rotate counter-clockwise: Top → Left → Bottom → Right → Top
            // When transitioning between horizontal and vertical, reverse the SOURCE index
            // Exception: For Right/Left faces (all vertical), reverse TARGET index instead
            // Exception: For Back face with swapped edges, reverse TARGET index
            for (int i = 0; i < 3; i++)
            {
                if (allVertical)
                {
                    // Special case for Right/Left faces: reverse target index
                    matrix[leftEdge[2 - i].Row, leftEdge[2 - i].Col] = topValues[i];
                    matrix[bottomEdge[2 - i].Row, bottomEdge[2 - i].Col] = leftValues[i];
                    matrix[rightEdge[2 - i].Row, rightEdge[2 - i].Col] = bottomValues[i];
                    matrix[topEdge[2 - i].Row, topEdge[2 - i].Col] = rightValues[i];
                }
                else if (isBackFacePattern)
                {
                    // Special case for Back face with swapped edges
                    // When Left/Right are swapped, the rotation is: Top → Right → Bottom → Left → Top
                    // But rightEdge parameter is actually LeftEdge, and leftEdge parameter is actually RightEdge
                    // Top → Right: Top → leftEdge (which is actually RightEdge)
                    matrix[leftEdge[i].Row, leftEdge[i].Col] = topValues[i];
                    // Right → Bottom: rightEdge (which is actually LeftEdge) → Bottom
                    matrix[bottomEdge[i].Row, bottomEdge[i].Col] = rightValues[i];
                    // Bottom → Left: Bottom → rightEdge (which is actually LeftEdge)
                    matrix[rightEdge[i].Row, rightEdge[i].Col] = bottomValues[i];
                    // Left → Top: leftEdge (which is actually RightEdge) → Top
                    matrix[topEdge[i].Row, topEdge[i].Col] = leftValues[i];
                }
                else
                {
                    // Normal case: reverse source if different orientation
                    int topSrcIdx = (topIsHorizontal != leftIsHorizontal) ? (2 - i) : i;
                    matrix[leftEdge[i].Row, leftEdge[i].Col] = topValues[topSrcIdx];
                    
                    int leftSrcIdx = (leftIsHorizontal != bottomIsHorizontal) ? (2 - i) : i;
                    matrix[bottomEdge[i].Row, bottomEdge[i].Col] = leftValues[leftSrcIdx];
                    
                    int bottomSrcIdx = (bottomIsHorizontal != rightIsHorizontal) ? (2 - i) : i;
                    matrix[rightEdge[i].Row, rightEdge[i].Col] = bottomValues[bottomSrcIdx];
                    
                    int rightSrcIdx = (rightIsHorizontal != topIsHorizontal) ? (2 - i) : i;
                    matrix[topEdge[i].Row, topEdge[i].Col] = rightValues[rightSrcIdx];
                }
            }
        }
    }
}
