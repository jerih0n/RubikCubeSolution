using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using RubikCubeSolution.Web.Models;
using RubikCubeSolution.Web.Services;

namespace RubikCubeSolution.Test.Services
{
    [TestClass]
    public class RubikCubeProcessorTests
    {
        [TestMethod]
        public void GetMatrix_ReturnsInitializedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);

            // Act
            var matrix = processor.GetMatrix();

            // Assert
            Assert.IsNotNull(matrix);
            Assert.AreEqual(9, matrix.GetLength(0));
            Assert.AreEqual(12, matrix.GetLength(1));
        }

        [TestMethod]
        public void Rotate_RightClockwise_ReturnsUpdatedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Right,
                Clockwise = true
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Rotate_UpperClockwise_ReturnsUpdatedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Upper,
                Clockwise = true
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Rotate_BottomClockwise_ReturnsUpdatedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Bottom,
                Clockwise = true
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Rotate_LeftClockwise_ReturnsUpdatedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Left,
                Clockwise = true
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Rotate_DownClockwise_ReturnsUpdatedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Down,
                Clockwise = true
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Rotate_InvalidSide_ThrowsArgumentException()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = (RubikCubeSideEnum)999,
                Clockwise = true
            };

            // Act
            processor.Rotate(request);
        }

        [TestMethod]
        public void Rotate_CounterClockwise_ReturnsUpdatedMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Front,
                Clockwise = false
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Reset_ReturnsOriginalMatrix()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var originalMatrix = processor.GetMatrix();

            // Rotate a few times
            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Front, Clockwise = true });
            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Right, Clockwise = true });

            // Act
            var resetMatrix = processor.Reset();

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, resetMatrix),
                "Reset should return the cube to its original state");
        }

        [TestMethod]
        public void Rotate_ConvertMatrixToArray_ReturnsCorrectFormat()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Front,
                Clockwise = true
            };

            // Act
            var result = processor.Rotate(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result.Length);
            Assert.AreEqual(12, result[0].Length);
        }

        private bool AreMatricesEqual(MatrixCellFillEnum[,] matrix1, int[][] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.Length)
                return false;

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                if (matrix1.GetLength(1) != matrix2[i].Length)
                    return false;

                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if ((int)matrix1[i, j] != matrix2[i][j])
                        return false;
                }
            }
            return true;
        }
    }
}