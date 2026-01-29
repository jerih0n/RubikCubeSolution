using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Test.Models
{
    [TestClass]
    public class RubikCubeTests
    {
        [TestMethod]
        public void Constructor_InitializesCube_MatrixHasCorrectSize()
        {
            // Arrange & Act
            var cube = new RubikCube();

            // Assert
            Assert.IsNotNull(cube.Matrix);
            Assert.AreEqual(MatrixConstants.ROWS_COUNT, cube.Matrix.GetLength(0));
            Assert.AreEqual(MatrixConstants.COLUMNS_COUNT, cube.Matrix.GetLength(1));
        }

        [TestMethod]
        public void Constructor_InitializesCube_MatrixIsInitialized()
        {
            // Arrange & Act
            var cube = new RubikCube();

            // Assert
            var hasNonNoneValues = false;
            for (int i = 0; i < cube.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < cube.Matrix.GetLength(1); j++)
                {
                    if (cube.Matrix[i, j] != MatrixCellFillEnum.None)
                    {
                        hasNonNoneValues = true;
                        break;
                    }
                }
                if (hasNonNoneValues) break;
            }
            Assert.IsTrue(hasNonNoneValues, "Cube should have initialized color values");
        }

        [TestMethod]
        public void RotateFront_Clockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateFront(true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateFront_CounterClockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateFront(false);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateRight_Clockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateRight(true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateUpper_Clockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateUpper(true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateBottom_Clockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateBottom(true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateLeft_Clockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateLeft(true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void RotateDown_Clockwise_ChangesMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.RotateDown(true);

            // Assert
            Assert.IsFalse(AreMatricesEqual(originalMatrix, cube.Matrix), "Matrix should change after rotation");
        }

        [TestMethod]
        public void Rotate_ClockwiseThenCounterClockwise_ReturnsToOriginalState()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.Rotate(RubikCubeSideEnum.Front, true);
            cube.Rotate(RubikCubeSideEnum.Front, false);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, cube.Matrix), 
                "Rotating clockwise then counter-clockwise should return to original state");
        }

        [TestMethod]
        public void Rotate_FourTimesClockwise_ReturnsToOriginalState()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();

            // Act
            cube.Rotate(RubikCubeSideEnum.Front, true);
            cube.Rotate(RubikCubeSideEnum.Front, true);
            cube.Rotate(RubikCubeSideEnum.Front, true);
            cube.Rotate(RubikCubeSideEnum.Front, true);

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, cube.Matrix), 
                "Rotating four times clockwise should return to original state");
        }

        [TestMethod]
        public void Reset_RestoresOriginalState()
        {
            // Arrange
            var cube = new RubikCube();
            var originalMatrix = (MatrixCellFillEnum[,])cube.Matrix.Clone();
            cube.RotateFront(true);
            cube.RotateRight(true);
            cube.RotateUpper(true);

            // Act
            cube.Reset();

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, cube.Matrix), 
                "Reset should restore the cube to its original state");
        }

        [TestMethod]
        public void PrintMatrix_ReturnsNonEmptyString()
        {
            // Arrange
            var cube = new RubikCube();

            // Act
            var result = cube.PrintMatrix();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
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
