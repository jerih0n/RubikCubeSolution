using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Logic.Helpers
{
    /// <summary>
    /// Provides primitive rotation operations for Rubik's Cube transformations.
    /// Uses a composition-based approach where complex face rotations are built from primitive operations.
    /// </summary>
    public static class PrimitiveRotationHelpers
    {
        /// <summary>
        /// Converts a sticker position index (1-54) to matrix coordinates (row, col).
        /// The sticker positions follow a standard Rubik's Cube face ordering:
        /// Upper (1-9), Right (10-18), Front (19-27), Down (28-36), Left (37-45), Back (46-54)
        /// </summary>
        private static (int row, int col) GetMatrixCoordinates(int stickerPosition)
        {
            // Upper face: rows 0-2, columns 3-5
            if (stickerPosition >= 1 && stickerPosition <= 9)
            {
                int position = stickerPosition - 1; // Convert to 0-indexed
                int row = position / 3;
                int col = 3 + (position % 3);
                return (row, col);
            }
            // Right face: rows 3-5, columns 6-8
            else if (stickerPosition >= 10 && stickerPosition <= 18)
            {
                int position = stickerPosition - 10;
                int row = 3 + (position / 3);
                int col = 6 + (position % 3);
                return (row, col);
            }
            // Front face: rows 3-5, columns 3-5
            else if (stickerPosition >= 19 && stickerPosition <= 27)
            {
                int position = stickerPosition - 19;
                int row = 3 + (position / 3);
                int col = 3 + (position % 3);
                return (row, col);
            }
            // Down face: rows 6-8, columns 3-5
            else if (stickerPosition >= 28 && stickerPosition <= 36)
            {
                int position = stickerPosition - 28;
                int row = 6 + (position / 3);
                int col = 3 + (position % 3);
                return (row, col);
            }
            // Left face: rows 3-5, columns 0-2
            else if (stickerPosition >= 37 && stickerPosition <= 45)
            {
                int position = stickerPosition - 37;
                int row = 3 + (position / 3);
                int col = position % 3;
                return (row, col);
            }
            // Back face: rows 3-5, columns 9-11
            else if (stickerPosition >= 46 && stickerPosition <= 54)
            {
                int position = stickerPosition - 46;
                int row = 3 + (position / 3);
                int col = 9 + (position % 3);
                return (row, col);
            }

            throw new ArgumentException($"Invalid sticker position: {stickerPosition}. Must be between 1 and 54.");
        }

        private static void SwapStickers(MatrixCellFillEnum[,] matrix, int position1, int position2)
        {
            var (row1, col1) = GetMatrixCoordinates(position1);
            var (row2, col2) = GetMatrixCoordinates(position2);
            var temp = matrix[row1, col1];
            matrix[row1, col1] = matrix[row2, col2];
            matrix[row2, col2] = temp;
        }

        private static MatrixCellFillEnum GetStickerValue(MatrixCellFillEnum[,] matrix, int position)
        {
            var (row, col) = GetMatrixCoordinates(position);
            return matrix[row, col];
        }

        private static void SetStickerValue(MatrixCellFillEnum[,] matrix, int position, MatrixCellFillEnum value)
        {
            var (row, col) = GetMatrixCoordinates(position);
            matrix[row, col] = value;
        }

        /// <summary>
        /// Primitive operation: Rotates the cube orientation around the vertical axis.
        /// This operation changes which face is considered "front" without rotating any face.
        /// </summary>
        private static void RotateCubeOrientation(MatrixCellFillEnum[,] matrix)
        {
            // Rotate the top row of edges around the cube
            SwapStickers(matrix, 1, 45);
            SwapStickers(matrix, 4, 42);
            SwapStickers(matrix, 7, 39);
            SwapStickers(matrix, 45, 46);
            SwapStickers(matrix, 42, 49);
            SwapStickers(matrix, 39, 52);
            SwapStickers(matrix, 46, 19);
            SwapStickers(matrix, 49, 22);
            SwapStickers(matrix, 52, 25);
            SwapStickers(matrix, 19, 1);
            SwapStickers(matrix, 22, 4);
            SwapStickers(matrix, 25, 7);

            // Rotate the middle row of edges
            SwapStickers(matrix, 2, 44);
            SwapStickers(matrix, 5, 41);
            SwapStickers(matrix, 8, 38);
            SwapStickers(matrix, 44, 47);
            SwapStickers(matrix, 41, 50);
            SwapStickers(matrix, 38, 53);
            SwapStickers(matrix, 47, 20);
            SwapStickers(matrix, 50, 23);
            SwapStickers(matrix, 53, 26);
            SwapStickers(matrix, 20, 2);
            SwapStickers(matrix, 23, 5);
            SwapStickers(matrix, 26, 8);

            // Rotate the bottom row of edges
            SwapStickers(matrix, 3, 43);
            SwapStickers(matrix, 6, 40);
            SwapStickers(matrix, 9, 37);
            SwapStickers(matrix, 43, 48);
            SwapStickers(matrix, 40, 51);
            SwapStickers(matrix, 37, 54);
            SwapStickers(matrix, 48, 21);
            SwapStickers(matrix, 51, 24);
            SwapStickers(matrix, 54, 27);
            SwapStickers(matrix, 21, 3);
            SwapStickers(matrix, 24, 6);
            SwapStickers(matrix, 27, 9);

            // Rotate the Right face center clockwise
            var temp1 = GetStickerValue(matrix, 10);
            var temp2 = GetStickerValue(matrix, 11);
            SetStickerValue(matrix, 10, GetStickerValue(matrix, 16));
            SetStickerValue(matrix, 11, GetStickerValue(matrix, 13));
            SetStickerValue(matrix, 16, GetStickerValue(matrix, 18));
            SetStickerValue(matrix, 13, GetStickerValue(matrix, 17));
            SetStickerValue(matrix, 18, GetStickerValue(matrix, 12));
            SetStickerValue(matrix, 17, GetStickerValue(matrix, 15));
            SetStickerValue(matrix, 12, temp1);
            SetStickerValue(matrix, 15, temp2);

            // Rotate the Back face center clockwise
            temp1 = GetStickerValue(matrix, 28);
            temp2 = GetStickerValue(matrix, 29);
            SetStickerValue(matrix, 28, GetStickerValue(matrix, 30));
            SetStickerValue(matrix, 29, GetStickerValue(matrix, 33));
            SetStickerValue(matrix, 30, GetStickerValue(matrix, 36));
            SetStickerValue(matrix, 33, GetStickerValue(matrix, 35));
            SetStickerValue(matrix, 36, GetStickerValue(matrix, 34));
            SetStickerValue(matrix, 35, GetStickerValue(matrix, 31));
            SetStickerValue(matrix, 34, temp1);
            SetStickerValue(matrix, 31, temp2);
        }

        /// <summary>
        /// Primitive operation: Rotates the Down face clockwise.
        /// This is the fundamental rotation operation used to compose other face rotations.
        /// </summary>
        private static void RotateDownFaceClockwise(MatrixCellFillEnum[,] matrix)
        {
            // Rotate edges around the Down face
            var temp1 = GetStickerValue(matrix, 25);
            var temp2 = GetStickerValue(matrix, 26);
            var temp3 = GetStickerValue(matrix, 27);
            SetStickerValue(matrix, 25, GetStickerValue(matrix, 16));
            SetStickerValue(matrix, 26, GetStickerValue(matrix, 17));
            SetStickerValue(matrix, 27, GetStickerValue(matrix, 18));
            SetStickerValue(matrix, 16, GetStickerValue(matrix, 43));
            SetStickerValue(matrix, 17, GetStickerValue(matrix, 44));
            SetStickerValue(matrix, 18, GetStickerValue(matrix, 45));
            SetStickerValue(matrix, 43, GetStickerValue(matrix, 34));
            SetStickerValue(matrix, 44, GetStickerValue(matrix, 35));
            SetStickerValue(matrix, 45, GetStickerValue(matrix, 36));
            SetStickerValue(matrix, 34, temp1);
            SetStickerValue(matrix, 35, temp2);
            SetStickerValue(matrix, 36, temp3);

            // Rotate the Down face center clockwise
            temp1 = GetStickerValue(matrix, 46);
            temp2 = GetStickerValue(matrix, 47);
            SetStickerValue(matrix, 46, GetStickerValue(matrix, 52));
            SetStickerValue(matrix, 47, GetStickerValue(matrix, 49));
            SetStickerValue(matrix, 52, GetStickerValue(matrix, 54));
            SetStickerValue(matrix, 49, GetStickerValue(matrix, 53));
            SetStickerValue(matrix, 54, GetStickerValue(matrix, 48));
            SetStickerValue(matrix, 53, GetStickerValue(matrix, 51));
            SetStickerValue(matrix, 48, temp1);
            SetStickerValue(matrix, 51, temp2);
        }

        /// <summary>
        /// Primitive operation: Rotates the Down face counter-clockwise.
        /// This is the inverse of RotateDownFaceClockwise.
        /// </summary>
        private static void RotateDownFaceCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            // Rotate edges around the Down face (reverse direction)
            var temp1 = GetStickerValue(matrix, 25);
            var temp2 = GetStickerValue(matrix, 26);
            var temp3 = GetStickerValue(matrix, 27);
            SetStickerValue(matrix, 25, GetStickerValue(matrix, 34));
            SetStickerValue(matrix, 26, GetStickerValue(matrix, 35));
            SetStickerValue(matrix, 27, GetStickerValue(matrix, 36));
            SetStickerValue(matrix, 34, GetStickerValue(matrix, 43));
            SetStickerValue(matrix, 35, GetStickerValue(matrix, 44));
            SetStickerValue(matrix, 36, GetStickerValue(matrix, 45));
            SetStickerValue(matrix, 43, GetStickerValue(matrix, 16));
            SetStickerValue(matrix, 44, GetStickerValue(matrix, 17));
            SetStickerValue(matrix, 45, GetStickerValue(matrix, 18));
            SetStickerValue(matrix, 16, temp1);
            SetStickerValue(matrix, 17, temp2);
            SetStickerValue(matrix, 18, temp3);

            // Rotate the Down face center counter-clockwise
            temp1 = GetStickerValue(matrix, 46);
            temp2 = GetStickerValue(matrix, 47);
            SetStickerValue(matrix, 46, GetStickerValue(matrix, 48));
            SetStickerValue(matrix, 47, GetStickerValue(matrix, 51));
            SetStickerValue(matrix, 48, GetStickerValue(matrix, 54));
            SetStickerValue(matrix, 51, GetStickerValue(matrix, 53));
            SetStickerValue(matrix, 54, GetStickerValue(matrix, 52));
            SetStickerValue(matrix, 53, GetStickerValue(matrix, 49));
            SetStickerValue(matrix, 52, temp1);
            SetStickerValue(matrix, 49, temp2);
        }

        /// <summary>
        /// Primitive operation: Rotates the cube forward (changes orientation).
        /// This operation tilts the cube forward, changing which face is considered "up".
        /// </summary>
        private static void RotateCubeForward(MatrixCellFillEnum[,] matrix)
        {
            // Rotate the front column of edges
            var temp1 = GetStickerValue(matrix, 19);
            var temp2 = GetStickerValue(matrix, 20);
            var temp3 = GetStickerValue(matrix, 21);
            SetStickerValue(matrix, 19, GetStickerValue(matrix, 28));
            SetStickerValue(matrix, 20, GetStickerValue(matrix, 29));
            SetStickerValue(matrix, 21, GetStickerValue(matrix, 30));
            SetStickerValue(matrix, 28, GetStickerValue(matrix, 37));
            SetStickerValue(matrix, 29, GetStickerValue(matrix, 38));
            SetStickerValue(matrix, 30, GetStickerValue(matrix, 39));
            SetStickerValue(matrix, 37, GetStickerValue(matrix, 10));
            SetStickerValue(matrix, 38, GetStickerValue(matrix, 11));
            SetStickerValue(matrix, 39, GetStickerValue(matrix, 12));
            SetStickerValue(matrix, 10, temp1);
            SetStickerValue(matrix, 11, temp2);
            SetStickerValue(matrix, 12, temp3);

            // Rotate the middle column of edges
            temp1 = GetStickerValue(matrix, 22);
            temp2 = GetStickerValue(matrix, 23);
            temp3 = GetStickerValue(matrix, 24);
            SetStickerValue(matrix, 22, GetStickerValue(matrix, 31));
            SetStickerValue(matrix, 23, GetStickerValue(matrix, 32));
            SetStickerValue(matrix, 24, GetStickerValue(matrix, 33));
            SetStickerValue(matrix, 31, GetStickerValue(matrix, 40));
            SetStickerValue(matrix, 32, GetStickerValue(matrix, 41));
            SetStickerValue(matrix, 33, GetStickerValue(matrix, 42));
            SetStickerValue(matrix, 40, GetStickerValue(matrix, 13));
            SetStickerValue(matrix, 41, GetStickerValue(matrix, 14));
            SetStickerValue(matrix, 42, GetStickerValue(matrix, 15));
            SetStickerValue(matrix, 13, temp1);
            SetStickerValue(matrix, 14, temp2);
            SetStickerValue(matrix, 15, temp3);

            // Rotate the back column of edges
            temp1 = GetStickerValue(matrix, 25);
            temp2 = GetStickerValue(matrix, 26);
            temp3 = GetStickerValue(matrix, 27);
            SetStickerValue(matrix, 25, GetStickerValue(matrix, 34));
            SetStickerValue(matrix, 26, GetStickerValue(matrix, 35));
            SetStickerValue(matrix, 27, GetStickerValue(matrix, 36));
            SetStickerValue(matrix, 34, GetStickerValue(matrix, 43));
            SetStickerValue(matrix, 35, GetStickerValue(matrix, 44));
            SetStickerValue(matrix, 36, GetStickerValue(matrix, 45));
            SetStickerValue(matrix, 43, GetStickerValue(matrix, 16));
            SetStickerValue(matrix, 44, GetStickerValue(matrix, 17));
            SetStickerValue(matrix, 45, GetStickerValue(matrix, 18));
            SetStickerValue(matrix, 16, temp1);
            SetStickerValue(matrix, 17, temp2);
            SetStickerValue(matrix, 18, temp3);

            // Rotate the Upper face center clockwise
            temp1 = GetStickerValue(matrix, 1);
            temp2 = GetStickerValue(matrix, 2);
            SetStickerValue(matrix, 1, GetStickerValue(matrix, 7));
            SetStickerValue(matrix, 2, GetStickerValue(matrix, 4));
            SetStickerValue(matrix, 7, GetStickerValue(matrix, 9));
            SetStickerValue(matrix, 4, GetStickerValue(matrix, 8));
            SetStickerValue(matrix, 9, GetStickerValue(matrix, 3));
            SetStickerValue(matrix, 8, GetStickerValue(matrix, 6));
            SetStickerValue(matrix, 3, temp1);
            SetStickerValue(matrix, 6, temp2);

            // Rotate the Back face center clockwise
            temp1 = GetStickerValue(matrix, 46);
            temp2 = GetStickerValue(matrix, 47);
            SetStickerValue(matrix, 46, GetStickerValue(matrix, 48));
            SetStickerValue(matrix, 47, GetStickerValue(matrix, 51));
            SetStickerValue(matrix, 48, GetStickerValue(matrix, 54));
            SetStickerValue(matrix, 51, GetStickerValue(matrix, 53));
            SetStickerValue(matrix, 54, GetStickerValue(matrix, 52));
            SetStickerValue(matrix, 53, GetStickerValue(matrix, 49));
            SetStickerValue(matrix, 52, temp1);
            SetStickerValue(matrix, 49, temp2);
        }

        // Face rotation functions composed from primitive operations

        /// <summary>
        /// Rotates the Upper face clockwise.
        /// Composition: RotateCubeOrientation × 2, RotateDownFaceClockwise, RotateCubeOrientation × 2
        /// </summary>
        public static void RotateUpperClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
        }

        /// <summary>
        /// Rotates the Upper face counter-clockwise.
        /// Composition: RotateCubeOrientation × 2, RotateDownFaceCounterClockwise, RotateCubeOrientation × 2
        /// </summary>
        public static void RotateUpperCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
        }

        /// <summary>
        /// Rotates the Front face clockwise.
        /// Composition: RotateCubeOrientation, RotateDownFaceClockwise, RotateCubeOrientation × 3
        /// </summary>
        public static void RotateFrontClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
        }

        /// <summary>
        /// Rotates the Front face counter-clockwise.
        /// Composition: RotateCubeOrientation, RotateDownFaceCounterClockwise, RotateCubeOrientation × 3
        /// </summary>
        public static void RotateFrontCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
        }

        /// <summary>
        /// Rotates the Right face clockwise.
        /// Composition: RotateCubeForward, RotateCubeOrientation, RotateDownFaceClockwise, RotateCubeOrientation × 3, RotateCubeForward × 3
        /// </summary>
        public static void RotateRightClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeForward(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
        }

        /// <summary>
        /// Rotates the Right face counter-clockwise.
        /// Composition: RotateCubeForward, RotateCubeOrientation, RotateDownFaceCounterClockwise, RotateCubeOrientation × 3, RotateCubeForward × 3
        /// </summary>
        public static void RotateRightCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeForward(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
        }

        /// <summary>
        /// Rotates the Left face clockwise.
        /// Composition: RotateCubeForward × 3, RotateCubeOrientation, RotateDownFaceClockwise, RotateCubeOrientation × 3, RotateCubeForward
        /// </summary>
        public static void RotateLeftClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeForward(matrix);
        }

        /// <summary>
        /// Rotates the Left face counter-clockwise.
        /// Composition: RotateCubeForward × 3, RotateCubeOrientation, RotateDownFaceCounterClockwise, RotateCubeOrientation × 3, RotateCubeForward
        /// </summary>
        public static void RotateLeftCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
            RotateCubeForward(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeForward(matrix);
        }

        /// <summary>
        /// Rotates the Down face clockwise.
        /// This is a direct primitive operation.
        /// </summary>
        public static void RotateDownClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateDownFaceClockwise(matrix);
        }

        /// <summary>
        /// Rotates the Down face counter-clockwise.
        /// This is a direct primitive operation.
        /// </summary>
        public static void RotateDownCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateDownFaceCounterClockwise(matrix);
        }

        /// <summary>
        /// Rotates the Back face clockwise.
        /// Composition: RotateCubeOrientation × 3, RotateDownFaceClockwise, RotateCubeOrientation
        /// </summary>
        public static void RotateBackClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RotateCubeOrientation(matrix);
        }

        /// <summary>
        /// Rotates the Back face counter-clockwise.
        /// Composition: RotateCubeOrientation × 3, RotateDownFaceCounterClockwise, RotateCubeOrientation
        /// </summary>
        public static void RotateBackCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RotateCubeOrientation(matrix);
        }
    }
}
