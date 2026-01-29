using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Helpers;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Test.Helpers
{
    [TestClass]
    public class RotationHelpersTests
    {
        [TestMethod]
        public void RotateLayer_FrontClockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var matrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateLayer_FrontCounterClockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var matrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, false);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateLayer_ClockwiseThenCounterClockwise_ReturnsToOriginal()
        {
            // Arrange
            var cube = new RubikCube();
            var matrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, true);
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, false);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, matrix), 
                "Rotating clockwise then counter-clockwise should return to original");
        }

        [TestMethod]
        public void RotateLayer_FourTimesClockwise_ReturnsToOriginal()
        {
            // Arrange
            var cube = new RubikCube();
            var matrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, true);
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, true);
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, true);
            RotationHelpers.RotateLayer(matrix, RubikCubeSideEnum.Front, true);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, matrix), 
                "Rotating four times clockwise should return to original");
        }

        [TestMethod]
        public void RotateLayer_AllSides_EachChangesMatrix()
        {
            // Arrange
            var sides = new[]
            {
                RubikCubeSideEnum.Front,
                RubikCubeSideEnum.Right,
                RubikCubeSideEnum.Upper,
                RubikCubeSideEnum.Bottom,
                RubikCubeSideEnum.Left,
                RubikCubeSideEnum.Down
            };

            foreach (var side in sides)
            {
                var cube = new RubikCube();
                var matrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();
                var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

                // Act
                RotationHelpers.RotateLayer(matrix, side, true);

                // Assert
                Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                    $"Rotating {side} should change the matrix");
            }
        }

        [TestMethod]
        public void RotateFaceClockwise_Rotates3x3Face()
        {
            // Arrange
            var matrix = new MatrixCellFillEnum[3, 3];
            matrix[0, 0] = MatrixCellFillEnum.Red;
            matrix[0, 1] = MatrixCellFillEnum.Green;
            matrix[0, 2] = MatrixCellFillEnum.Blue;
            matrix[1, 0] = MatrixCellFillEnum.White;
            matrix[1, 1] = MatrixCellFillEnum.Orange;
            matrix[1, 2] = MatrixCellFillEnum.Yellow;
            matrix[2, 0] = MatrixCellFillEnum.Red;
            matrix[2, 1] = MatrixCellFillEnum.Green;
            matrix[2, 2] = MatrixCellFillEnum.Blue;

            var originalTopLeft = matrix[0, 0];
            var originalTopRight = matrix[0, 2];

            // Act
            RotationHelpers.RotateFaceClockwise(matrix, 0, 2, 0, 2);

            // Assert
            Assert.AreEqual(originalTopLeft, matrix[0, 2], "Top-left should move to top-right");
            Assert.AreEqual(originalTopRight, matrix[2, 2], "Top-right should move to bottom-right");
        }

        [TestMethod]
        public void RotateFaceCounterClockwise_Rotates3x3Face()
        {
            // Arrange
            var matrix = new MatrixCellFillEnum[3, 3];
            matrix[0, 0] = MatrixCellFillEnum.Red;
            matrix[0, 1] = MatrixCellFillEnum.Green;
            matrix[0, 2] = MatrixCellFillEnum.Blue;
            matrix[1, 0] = MatrixCellFillEnum.White;
            matrix[1, 1] = MatrixCellFillEnum.Orange;
            matrix[1, 2] = MatrixCellFillEnum.Yellow;
            matrix[2, 0] = MatrixCellFillEnum.Red;
            matrix[2, 1] = MatrixCellFillEnum.Green;
            matrix[2, 2] = MatrixCellFillEnum.Blue;

            var originalTopLeft = matrix[0, 0];
            var originalTopRight = matrix[0, 2];

            // Act
            RotationHelpers.RotateFaceCounterClockwise(matrix, 0, 2, 0, 2);

            // Assert
            Assert.AreEqual(originalTopLeft, matrix[2, 0], "Top-left should move to bottom-left");
            Assert.AreEqual(originalTopRight, matrix[0, 0], "Top-right should move to top-left");
        }

        [TestMethod]
        public void RotateFaceClockwise_FourTimes_ReturnsToOriginal()
        {
            // Arrange
            var matrix = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = (MatrixCellFillEnum)(i * 3 + j + 1);
                }
            }
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            RotationHelpers.RotateFaceClockwise(matrix, 0, 2, 0, 2);
            RotationHelpers.RotateFaceClockwise(matrix, 0, 2, 0, 2);
            RotationHelpers.RotateFaceClockwise(matrix, 0, 2, 0, 2);
            RotationHelpers.RotateFaceClockwise(matrix, 0, 2, 0, 2);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, matrix), 
                "Rotating four times clockwise should return to original");
        }

        private bool AreMatricesEqual(MatrixCellFillEnum[,] matrix1, MatrixCellFillEnum[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || 
                matrix1.GetLength(1) != matrix2.GetLength(1))
                return false;

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                        return false;
                }
            }
            return true;
        }
    }
}
