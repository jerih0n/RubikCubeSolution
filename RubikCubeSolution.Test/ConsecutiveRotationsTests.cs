using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using RubikCubeSolution.Web.Models;
using RubikCubeSolution.Web.Services;

namespace RubikCubeSolution.Test
{
    [TestClass]
    public class ConsecutiveRotationsTests
    {
        [TestMethod]
        public void MultipleRotations_OppositeRotations_ReturnsToOriginal()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var originalMatrix = processor.GetMatrix();

            // Act - Perform rotations and their opposites
            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Front, Clockwise = true });
            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Front, Clockwise = false });

            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Right, Clockwise = true });
            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Right, Clockwise = false });

            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Upper, Clockwise = true });
            processor.Rotate(new RotationRequest { Side = RubikCubeSideEnum.Upper, Clockwise = false });

            var finalMatrix = processor.GetMatrix();

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, finalMatrix),
                "Opposite rotations should return to original state");
        }

        [TestMethod]
        public void MultipleRotations_FourTimesEachSide_ReturnsToOriginal()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var originalMatrix = processor.GetMatrix();

            var sides = new[]
            {
                RubikCubeSideEnum.Front,
                RubikCubeSideEnum.Right,
                RubikCubeSideEnum.Upper,
                RubikCubeSideEnum.Bottom,
                RubikCubeSideEnum.Left,
                RubikCubeSideEnum.Down
            };

            // Act - Rotate each side 4 times clockwise
            foreach (var side in sides)
            {
                processor.Rotate(new RotationRequest { Side = side, Clockwise = true });
                processor.Rotate(new RotationRequest { Side = side, Clockwise = true });
                processor.Rotate(new RotationRequest { Side = side, Clockwise = true });
                processor.Rotate(new RotationRequest { Side = side, Clockwise = true });
            }

            var finalMatrix = processor.GetMatrix();

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, finalMatrix),
                "Rotating each side 4 times should return to original state");
        }

        [TestMethod]
        public void MultipleRotations_AllSidesClockwiseThenCounterClockwise_ReturnsToOriginal()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);
            var originalMatrix = processor.GetMatrix();

            var sides = new[]
            {
                RubikCubeSideEnum.Front,
                RubikCubeSideEnum.Right,
                RubikCubeSideEnum.Upper,
                RubikCubeSideEnum.Bottom,
                RubikCubeSideEnum.Left,
                RubikCubeSideEnum.Down
            };

            // Act - Rotate all sides clockwise
            foreach (var side in sides)
            {
                processor.Rotate(new RotationRequest { Side = side, Clockwise = true });
            }

            // Then rotate all sides counter-clockwise in reverse order
            for (int i = sides.Length - 1; i >= 0; i--)
            {
                processor.Rotate(new RotationRequest { Side = sides[i], Clockwise = false });
            }

            var finalMatrix = processor.GetMatrix();

            // Assert
            Assert.IsTrue(AreMatricesEqual(originalMatrix, finalMatrix),
                "Rotating all sides clockwise then counter-clockwise should return to original");
        }

        [TestMethod]
        public void MultipleRotations_LongSequence_NoExceptions()
        {
            // Arrange
            var cube = new RubikCube();
            var processor = new RubikCubeProcessor(cube);

            // Act & Assert - Perform many rotations without exceptions
            for (int i = 0; i < 100; i++)
            {
                var side = (RubikCubeSideEnum)((i % 6) + 1);
                var clockwise = i % 2 == 0;

                var result = processor.Rotate(new RotationRequest
                {
                    Side = side,
                    Clockwise = clockwise
                });

                Assert.IsNotNull(result, $"Rotation {i} should not return null");
            }
        }

        [TestMethod]
        public void MultipleRotations_ComplexAlgorithm_ConsistentResults()
        {
            // Arrange
            var cube1 = new RubikCube();
            var processor1 = new RubikCubeProcessor(cube1);

            var cube2 = new RubikCube();
            var processor2 = new RubikCubeProcessor(cube2);

            // Act - Perform same sequence on both cubes
            var sequence = new[]
            {
                new RotationRequest { Side = RubikCubeSideEnum.Front, Clockwise = true },
                new RotationRequest { Side = RubikCubeSideEnum.Right, Clockwise = true },
                new RotationRequest { Side = RubikCubeSideEnum.Upper, Clockwise = false },
                new RotationRequest { Side = RubikCubeSideEnum.Front, Clockwise = false },
                new RotationRequest { Side = RubikCubeSideEnum.Right, Clockwise = false },
                new RotationRequest { Side = RubikCubeSideEnum.Upper, Clockwise = true },
            };

            foreach (var rotation in sequence)
            {
                processor1.Rotate(rotation);
                processor2.Rotate(rotation);
            }

            var matrix1 = processor1.GetMatrix();
            var matrix2 = processor2.GetMatrix();

            // Assert
            Assert.IsTrue(AreMatricesEqual(matrix1, matrix2),
                "Same sequence should produce same results");
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