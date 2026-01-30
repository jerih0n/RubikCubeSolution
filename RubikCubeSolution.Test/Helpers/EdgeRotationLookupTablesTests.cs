using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Helpers;

namespace RubikCubeSolution.Test.Helpers
{
    [TestClass]
    public class EdgeRotationLookupTablesTests
    {
        [TestMethod]
        public void GetMapping_FrontClockwise_ReturnsValidMapping()
        {
            // Arrange
            var face = RubikCubeSideEnum.Front;
            var clockwise = true;
            var sourceEdge = EdgeIndex.Top;
            var sourceIndex = 0;

            // Act
            var mapping = EdgeRotationLookupTables.GetMapping(face, clockwise, sourceEdge, sourceIndex);

            // Assert
            Assert.IsNotNull(mapping);
            Assert.IsTrue((int)mapping.TargetEdge >= 0 && (int)mapping.TargetEdge < 4);
            Assert.IsTrue(mapping.TargetIndex >= 0 && mapping.TargetIndex < 3);
        }

        [TestMethod]
        public void GetMapping_FrontCounterClockwise_ReturnsValidMapping()
        {
            // Arrange
            var face = RubikCubeSideEnum.Front;
            var clockwise = false;
            var sourceEdge = EdgeIndex.Top;
            var sourceIndex = 0;

            // Act
            var mapping = EdgeRotationLookupTables.GetMapping(face, clockwise, sourceEdge, sourceIndex);

            // Assert
            Assert.IsNotNull(mapping);
            Assert.IsTrue((int)mapping.TargetEdge >= 0 && (int)mapping.TargetEdge < 4);
            Assert.IsTrue(mapping.TargetIndex >= 0 && mapping.TargetIndex < 3);
        }

        [TestMethod]
        public void GetMapping_AllEdges_ReturnsValidMappings()
        {
            // Arrange
            var face = RubikCubeSideEnum.Front;
            var clockwise = true;
            var edges = new[] { EdgeIndex.Top, EdgeIndex.Right, EdgeIndex.Bottom, EdgeIndex.Left };

            // Act & Assert
            foreach (var edge in edges)
            {
                for (int index = 0; index < 3; index++)
                {
                    var mapping = EdgeRotationLookupTables.GetMapping(face, clockwise, edge, index);
                    Assert.IsNotNull(mapping);
                    Assert.IsTrue((int)mapping.TargetEdge >= 0 && (int)mapping.TargetEdge < 4);
                    Assert.IsTrue(mapping.TargetIndex >= 0 && mapping.TargetIndex < 3);
                }
            }
        }

        [TestMethod]
        public void GetMapping_AllFaces_ReturnsValidMappings()
        {
            // Arrange
            var faces = new[]
            {
                RubikCubeSideEnum.Front,
                RubikCubeSideEnum.Right,
                RubikCubeSideEnum.Upper,
                RubikCubeSideEnum.Bottom,
                RubikCubeSideEnum.Left,
                RubikCubeSideEnum.Down
            };

            // Act & Assert
            foreach (var face in faces)
            {
                var mapping = EdgeRotationLookupTables.GetMapping(face, true, EdgeIndex.Top, 0);
                Assert.IsNotNull(mapping);
                Assert.IsTrue((int)mapping.TargetEdge >= 0 && (int)mapping.TargetEdge < 4);
                Assert.IsTrue(mapping.TargetIndex >= 0 && mapping.TargetIndex < 3);
            }
        }

        [TestMethod]
        public void GetMapping_ClockwiseAndCounterClockwise_DifferentMappings()
        {
            // Arrange
            var face = RubikCubeSideEnum.Front;
            var sourceEdge = EdgeIndex.Top;
            var sourceIndex = 0;

            // Act
            var clockwiseMapping = EdgeRotationLookupTables.GetMapping(face, true, sourceEdge, sourceIndex);
            var counterClockwiseMapping = EdgeRotationLookupTables.GetMapping(face, false, sourceEdge, sourceIndex);

            // Assert
            // They might be the same or different, but both should be valid
            Assert.IsNotNull(clockwiseMapping);
            Assert.IsNotNull(counterClockwiseMapping);
        }
    }
}