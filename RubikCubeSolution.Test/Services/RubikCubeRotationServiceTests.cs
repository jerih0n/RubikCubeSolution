using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Services;

namespace RubikCubeSolution.Test.Services
{
    [TestClass]
    public class RubikCubeRotationServiceTests
    {
        [TestMethod]
        public void Rotate_FrontClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateFront(matrix, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_FrontCounterClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateFront(matrix, false);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_RightClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateRight(matrix, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_UpperClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateUpper(matrix, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_BottomClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateBottom(matrix, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_LeftClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateLeft(matrix, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_DownClockwise_ChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.RotateDown(matrix, true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_ClockwiseThenCounterClockwise_ReturnsToOriginal()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.Rotate(matrix, RubikCubeSideEnum.Front, true);
            service.Rotate(matrix, RubikCubeSideEnum.Front, false);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, matrix), 
                "Rotating clockwise then counter-clockwise should return to original");
        }

        [TestMethod]
        public void Rotate_FourTimesClockwise_ReturnsToOriginal()
        {
            // Arrange
            var service = new RubikCubeRotationService();
            var matrix = CreateInitializedMatrix();
            var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

            // Act
            service.Rotate(matrix, RubikCubeSideEnum.Front, true);
            service.Rotate(matrix, RubikCubeSideEnum.Front, true);
            service.Rotate(matrix, RubikCubeSideEnum.Front, true);
            service.Rotate(matrix, RubikCubeSideEnum.Front, true);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, matrix), 
                "Rotating four times clockwise should return to original");
        }

        [TestMethod]
        public void Rotate_AllSides_EachChangesMatrix()
        {
            // Arrange
            var service = new RubikCubeRotationService();
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
                var matrix = CreateInitializedMatrix();
                var originalMatrix = (MatrixCellFillEnum[,])matrix.Clone();

                // Act
                service.Rotate(matrix, side, true);

                // Assert
                Assert.IsFalse(AreMatricesEqual(originalMatrix, matrix), 
                    $"Rotating {side} should change the matrix");
            }
        }

        private MatrixCellFillEnum[,] CreateInitializedMatrix()
        {
            var cube = new Logic.Models.RubikCube();
            return cube.Matrix;
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
