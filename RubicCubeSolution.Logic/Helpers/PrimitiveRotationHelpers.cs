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
        #region Sticker Position Mapping

        /// <summary>
        /// Converts a sticker position index (1-54) to matrix coordinates (row, col).
        /// The sticker positions follow a standard Rubik's Cube face ordering:
        /// Upper (1-9), Right (10-18), Front (19-27), Down (28-36), Left (37-45), Back (46-54)
        /// </summary>
        private static (int row, int col) GetMatrixCoordinates(int stickerPosition)
        {
            return stickerPosition switch
            {
                >= 1 and <= 9 => ((stickerPosition - 1) / 3, 3 + ((stickerPosition - 1) % 3)),      // Upper
                >= 10 and <= 18 => (3 + ((stickerPosition - 10) / 3), 6 + ((stickerPosition - 10) % 3)), // Right
                >= 19 and <= 27 => (3 + ((stickerPosition - 19) / 3), 3 + ((stickerPosition - 19) % 3)), // Front
                >= 28 and <= 36 => (6 + ((stickerPosition - 28) / 3), 3 + ((stickerPosition - 28) % 3)), // Down
                >= 37 and <= 45 => (3 + ((stickerPosition - 37) / 3), (stickerPosition - 37) % 3),      // Left
                >= 46 and <= 54 => (3 + ((stickerPosition - 46) / 3), 9 + ((stickerPosition - 46) % 3)), // Back
                _ => throw new ArgumentException($"Invalid sticker position: {stickerPosition}. Must be between 1 and 54.")
            };
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

        #endregion

        #region Cycle Rotation Helpers

        /// <summary>
        /// Rotates a cycle of sticker positions. Each position moves to the next position in the cycle.
        /// The cycle array should contain positions in order: [pos1, pos2, pos3, pos4] where pos1→pos2→pos3→pos4→pos1
        /// </summary>
        private static void RotateCycle(MatrixCellFillEnum[,] matrix, int[] cycle)
        {
            if (cycle.Length < 2) return;

            var firstValue = GetStickerValue(matrix, cycle[0]);
            for (int i = 0; i < cycle.Length - 1; i++)
            {
                SetStickerValue(matrix, cycle[i], GetStickerValue(matrix, cycle[i + 1]));
            }
            SetStickerValue(matrix, cycle[cycle.Length - 1], firstValue);
        }

        /// <summary>
        /// Rotates a cycle of sticker positions in reverse direction.
        /// </summary>
        private static void RotateCycleReverse(MatrixCellFillEnum[,] matrix, int[] cycle)
        {
            if (cycle.Length < 2) return;

            var lastValue = GetStickerValue(matrix, cycle[cycle.Length - 1]);
            for (int i = cycle.Length - 1; i > 0; i--)
            {
                SetStickerValue(matrix, cycle[i], GetStickerValue(matrix, cycle[i - 1]));
            }
            SetStickerValue(matrix, cycle[0], lastValue);
        }

        /// <summary>
        /// Rotates a 3x3 face center clockwise using the standard corner and edge cycle pattern.
        /// For a 3x3 face with positions:
        ///   1 2 3
        ///   4 5 6
        ///   7 8 9
        /// Corners are: top-left(1), top-right(3), bottom-right(9), bottom-left(7)
        /// Edges are: top(2), right(6), bottom(8), left(4)
        /// </summary>
        private static void RotateFaceCenterClockwise(MatrixCellFillEnum[,] matrix, int basePosition)
        {
            // Calculate positions for a 3x3 face starting at basePosition
            // Positions: basePosition, basePosition+1, basePosition+2, ..., basePosition+8
            int topLeft = basePosition;
            int top = basePosition + 1;
            int topRight = basePosition + 2;
            int left = basePosition + 3;
            int center = basePosition + 4; // Not rotated
            int right = basePosition + 5;
            int bottomLeft = basePosition + 6;
            int bottom = basePosition + 7;
            int bottomRight = basePosition + 8;

            // Rotate corners: topLeft → topRight → bottomRight → bottomLeft → topLeft
            RotateCycle(matrix, new[] { topLeft, topRight, bottomRight, bottomLeft });
            
            // Rotate edges: top → right → bottom → left → top
            RotateCycle(matrix, new[] { top, right, bottom, left });
        }

        /// <summary>
        /// Rotates a 3x3 face center counter-clockwise.
        /// </summary>
        private static void RotateFaceCenterCounterClockwise(MatrixCellFillEnum[,] matrix, int basePosition)
        {
            int topLeft = basePosition;
            int top = basePosition + 1;
            int topRight = basePosition + 2;
            int left = basePosition + 3;
            int right = basePosition + 5;
            int bottomLeft = basePosition + 6;
            int bottom = basePosition + 7;
            int bottomRight = basePosition + 8;

            // Rotate corners in reverse: topLeft → bottomLeft → bottomRight → topRight → topLeft
            RotateCycleReverse(matrix, new[] { topLeft, topRight, bottomRight, bottomLeft });
            
            // Rotate edges in reverse: top → left → bottom → right → top
            RotateCycleReverse(matrix, new[] { top, right, bottom, left });
        }

        #endregion

        #region Primitive Operations

        /// <summary>
        /// Primitive operation: Rotates the cube orientation around the vertical axis.
        /// This operation changes which face is considered "front" without rotating any face.
        /// </summary>
        private static void RotateCubeOrientation(MatrixCellFillEnum[,] matrix)
        {
            // Rotate edge cycles around the cube (top, middle, bottom rows)
            RotateCycle(matrix, new[] { 1, 45, 46, 19 });
            RotateCycle(matrix, new[] { 2, 44, 47, 20 });
            RotateCycle(matrix, new[] { 3, 43, 48, 21 });
            RotateCycle(matrix, new[] { 4, 42, 49, 22 });
            RotateCycle(matrix, new[] { 5, 41, 50, 23 });
            RotateCycle(matrix, new[] { 6, 40, 51, 24 });
            RotateCycle(matrix, new[] { 7, 39, 52, 25 });
            RotateCycle(matrix, new[] { 8, 38, 53, 26 });
            RotateCycle(matrix, new[] { 9, 37, 54, 27 });

            // Rotate Right face center clockwise (base position 10: positions 10-18)
            RotateFaceCenterClockwise(matrix, 10);

            // Rotate Back face center clockwise (base position 28: positions 28-36)
            RotateFaceCenterClockwise(matrix, 28);
        }

        /// <summary>
        /// Primitive operation: Rotates the Down face clockwise.
        /// This is the fundamental rotation operation used to compose other face rotations.
        /// </summary>
        private static void RotateDownFaceClockwise(MatrixCellFillEnum[,] matrix)
        {
            // Rotate edges around the Down face
            RotateCycle(matrix, new[] { 25, 16, 43, 34 });
            RotateCycle(matrix, new[] { 26, 17, 44, 35 });
            RotateCycle(matrix, new[] { 27, 18, 45, 36 });

            // Rotate the Down face center clockwise (base position 46: positions 46-54)
            RotateFaceCenterClockwise(matrix, 46);
        }

        /// <summary>
        /// Primitive operation: Rotates the Down face counter-clockwise.
        /// This is the inverse of RotateDownFaceClockwise.
        /// </summary>
        private static void RotateDownFaceCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            // Rotate edges around the Down face (reverse direction)
            RotateCycleReverse(matrix, new[] { 25, 16, 43, 34 });
            RotateCycleReverse(matrix, new[] { 26, 17, 44, 35 });
            RotateCycleReverse(matrix, new[] { 27, 18, 45, 36 });

            // Rotate the Down face center counter-clockwise (base position 46: positions 46-54)
            RotateFaceCenterCounterClockwise(matrix, 46);
        }

        /// <summary>
        /// Primitive operation: Rotates the cube forward (changes orientation).
        /// This operation tilts the cube forward, changing which face is considered "up".
        /// </summary>
        private static void RotateCubeForward(MatrixCellFillEnum[,] matrix)
        {
            // Rotate edge columns (front, middle, back)
            RotateCycle(matrix, new[] { 19, 28, 37, 10 });
            RotateCycle(matrix, new[] { 20, 29, 38, 11 });
            RotateCycle(matrix, new[] { 21, 30, 39, 12 });
            RotateCycle(matrix, new[] { 22, 31, 40, 13 });
            RotateCycle(matrix, new[] { 23, 32, 41, 14 });
            RotateCycle(matrix, new[] { 24, 33, 42, 15 });
            RotateCycle(matrix, new[] { 25, 34, 43, 16 });
            RotateCycle(matrix, new[] { 26, 35, 44, 17 });
            RotateCycle(matrix, new[] { 27, 36, 45, 18 });

            // Rotate Upper face center clockwise (base position 1: positions 1-9)
            RotateFaceCenterClockwise(matrix, 1);

            // Rotate Back face center clockwise (base position 46: positions 46-54)
            RotateFaceCenterClockwise(matrix, 46);
        }

        #endregion

        #region Composition Helpers

        /// <summary>
        /// Executes a sequence of primitive operations.
        /// </summary>
        private static void ExecuteSequence(MatrixCellFillEnum[,] matrix, Action<MatrixCellFillEnum[,]>[] operations)
        {
            foreach (var operation in operations)
            {
                operation(matrix);
            }
        }

        /// <summary>
        /// Executes a primitive operation multiple times.
        /// </summary>
        private static void RepeatOperation(MatrixCellFillEnum[,] matrix, Action<MatrixCellFillEnum[,]> operation, int count)
        {
            for (int i = 0; i < count; i++)
            {
                operation(matrix);
            }
        }

        #endregion

        #region Public Face Rotation Methods

        /// <summary>
        /// Rotates the Upper face clockwise.
        /// Composition: RotateCubeOrientation × 2, RotateDownFaceClockwise, RotateCubeOrientation × 2
        /// </summary>
        public static void RotateUpperClockwise(MatrixCellFillEnum[,] matrix)
        {
            RepeatOperation(matrix, RotateCubeOrientation, 2);
            RotateDownFaceClockwise(matrix);
            RepeatOperation(matrix, RotateCubeOrientation, 2);
        }

        /// <summary>
        /// Rotates the Upper face counter-clockwise.
        /// Composition: RotateCubeOrientation × 2, RotateDownFaceCounterClockwise, RotateCubeOrientation × 2
        /// </summary>
        public static void RotateUpperCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RepeatOperation(matrix, RotateCubeOrientation, 2);
            RotateDownFaceCounterClockwise(matrix);
            RepeatOperation(matrix, RotateCubeOrientation, 2);
        }

        /// <summary>
        /// Rotates the Front face clockwise.
        /// Composition: RotateCubeOrientation, RotateDownFaceClockwise, RotateCubeOrientation × 3
        /// </summary>
        public static void RotateFrontClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RepeatOperation(matrix, RotateCubeOrientation, 3);
        }

        /// <summary>
        /// Rotates the Front face counter-clockwise.
        /// Composition: RotateCubeOrientation, RotateDownFaceCounterClockwise, RotateCubeOrientation × 3
        /// </summary>
        public static void RotateFrontCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RepeatOperation(matrix, RotateCubeOrientation, 3);
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
            RepeatOperation(matrix, RotateCubeOrientation, 3);
            RepeatOperation(matrix, RotateCubeForward, 3);
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
            RepeatOperation(matrix, RotateCubeOrientation, 3);
            RepeatOperation(matrix, RotateCubeForward, 3);
        }

        /// <summary>
        /// Rotates the Left face clockwise.
        /// Composition: RotateCubeForward × 3, RotateCubeOrientation, RotateDownFaceClockwise, RotateCubeOrientation × 3, RotateCubeForward
        /// </summary>
        public static void RotateLeftClockwise(MatrixCellFillEnum[,] matrix)
        {
            RepeatOperation(matrix, RotateCubeForward, 3);
            RotateCubeOrientation(matrix);
            RotateDownFaceClockwise(matrix);
            RepeatOperation(matrix, RotateCubeOrientation, 3);
            RotateCubeForward(matrix);
        }

        /// <summary>
        /// Rotates the Left face counter-clockwise.
        /// Composition: RotateCubeForward × 3, RotateCubeOrientation, RotateDownFaceCounterClockwise, RotateCubeOrientation × 3, RotateCubeForward
        /// </summary>
        public static void RotateLeftCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RepeatOperation(matrix, RotateCubeForward, 3);
            RotateCubeOrientation(matrix);
            RotateDownFaceCounterClockwise(matrix);
            RepeatOperation(matrix, RotateCubeOrientation, 3);
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
            RepeatOperation(matrix, RotateCubeOrientation, 3);
            RotateDownFaceClockwise(matrix);
            RotateCubeOrientation(matrix);
        }

        /// <summary>
        /// Rotates the Back face counter-clockwise.
        /// Composition: RotateCubeOrientation × 3, RotateDownFaceCounterClockwise, RotateCubeOrientation
        /// </summary>
        public static void RotateBackCounterClockwise(MatrixCellFillEnum[,] matrix)
        {
            RepeatOperation(matrix, RotateCubeOrientation, 3);
            RotateDownFaceCounterClockwise(matrix);
            RotateCubeOrientation(matrix);
        }

        #endregion
    }
}
