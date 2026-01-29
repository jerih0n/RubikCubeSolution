using RubikCube.Playground.Enums;
using System.Collections.Generic;

namespace RubikCube.Playground.Configuration
{
    public class FaceEdges
    {
        public List<Cell> TopEdge { get; set; } = new List<Cell>();
        public List<Cell> RightEdge { get; set; } = new List<Cell>();
        public List<Cell> BottomEdge { get; set; } = new List<Cell>();
        public List<Cell> LeftEdge { get; set; } = new List<Cell>();
    }

    public class EdgeMappingConfiguration
    {
        private readonly Dictionary<RubikCubeSideEnum, FaceEdges> _edgeMappings;

        public EdgeMappingConfiguration()
        {
            _edgeMappings = BuildEdgeMappings();
        }

        public FaceEdges GetEdgesForFace(RubikCubeSideEnum face)
        {
            return _edgeMappings[face];
        }

        private Dictionary<RubikCubeSideEnum, FaceEdges> BuildEdgeMappings()
        {
            var mappings = new Dictionary<RubikCubeSideEnum, FaceEdges>();

            // Front (F) - Green face (rows 3-5, columns 3-5)
            mappings[RubikCubeSideEnum.Front] = new FaceEdges
            {
                // Top edge: from Upper face bottom row (row 2, columns 3-5)
                TopEdge = new List<Cell> { new Cell(2, 3), new Cell(2, 4), new Cell(2, 5) },
                // Right edge: from Right face left column (rows 3-5, column 6)
                RightEdge = new List<Cell> { new Cell(3, 6), new Cell(4, 6), new Cell(5, 6) },
                // Bottom edge: from Down face top row (row 6, columns 3-5)
                BottomEdge = new List<Cell> { new Cell(6, 3), new Cell(6, 4), new Cell(6, 5) },
                // Left edge: from Left face right column (rows 3-5, column 2)
                LeftEdge = new List<Cell> { new Cell(3, 2), new Cell(4, 2), new Cell(5, 2) }
            };

            // Right (R) - Red face (rows 3-5, columns 6-8)
            mappings[RubikCubeSideEnum.Right] = new FaceEdges
            {
                // Top edge: from Upper face right column (rows 0-2, column 5)
                TopEdge = new List<Cell> { new Cell(0, 5), new Cell(1, 5), new Cell(2, 5) },
                // Back edge: from Bottom face left column (rows 3-5, column 9)
                RightEdge = new List<Cell> { new Cell(3, 9), new Cell(4, 9), new Cell(5, 9) },
                // Bottom edge: from Down face right column (rows 6-8, column 5)
                BottomEdge = new List<Cell> { new Cell(6, 5), new Cell(7, 5), new Cell(8, 5) },
                // Front edge: from Front face right column (rows 3-5, column 5)
                LeftEdge = new List<Cell> { new Cell(3, 5), new Cell(4, 5), new Cell(5, 5) }
            };

            // Upper (U) - White face (rows 0-2, columns 3-5)
            mappings[RubikCubeSideEnum.Upper] = new FaceEdges
            {
                // Back edge: from Bottom face top row (row 3, columns 9-11)
                TopEdge = new List<Cell> { new Cell(3, 9), new Cell(3, 10), new Cell(3, 11) },
                // Right edge: from Right face top row (row 3, columns 6-8)
                RightEdge = new List<Cell> { new Cell(3, 6), new Cell(3, 7), new Cell(3, 8) },
                // Front edge: from Front face top row (row 3, columns 3-5)
                BottomEdge = new List<Cell> { new Cell(3, 3), new Cell(3, 4), new Cell(3, 5) },
                // Left edge: from Left face top row (row 3, columns 0-2)
                LeftEdge = new List<Cell> { new Cell(3, 0), new Cell(3, 1), new Cell(3, 2) }
            };

            // Bottom/Back (B) - Blue face (rows 3-5, columns 9-11)
            // In the 2D net: Back is to the right of Right, and connects to Upper's top and Down's bottom
            mappings[RubikCubeSideEnum.Bottom] = new FaceEdges
            {
                // Top edge: from Upper face right column (rows 0-2, column 5) - wraps to Back's top
                TopEdge = new List<Cell> { new Cell(0, 5), new Cell(1, 5), new Cell(2, 5) },
                // Right edge: from Right face right column (rows 3-5, column 8) - adjacent to Back's left
                RightEdge = new List<Cell> { new Cell(3, 8), new Cell(4, 8), new Cell(5, 8) },
                // Bottom edge: from Down face right column (rows 6-8, column 5) - wraps to Back's bottom
                BottomEdge = new List<Cell> { new Cell(6, 5), new Cell(7, 5), new Cell(8, 5) },
                // Left edge: from Left face left column (rows 3-5, column 0) - wraps around to Back's right
                LeftEdge = new List<Cell> { new Cell(3, 0), new Cell(4, 0), new Cell(5, 0) }
            };

            // Left (L) - Orange face (rows 3-5, columns 0-2)
            mappings[RubikCubeSideEnum.Left] = new FaceEdges
            {
                // Top edge: from Upper face left column (rows 0-2, column 3)
                TopEdge = new List<Cell> { new Cell(0, 3), new Cell(1, 3), new Cell(2, 3) },
                // Front edge: from Front face left column (rows 3-5, column 3)
                RightEdge = new List<Cell> { new Cell(3, 3), new Cell(4, 3), new Cell(5, 3) },
                // Bottom edge: from Down face left column (rows 6-8, column 3)
                BottomEdge = new List<Cell> { new Cell(6, 3), new Cell(7, 3), new Cell(8, 3) },
                // Back edge: from Bottom face right column (rows 3-5, column 11)
                LeftEdge = new List<Cell> { new Cell(3, 11), new Cell(4, 11), new Cell(5, 11) }
            };

            // Down (D) - Yellow face (rows 6-8, columns 3-5)
            mappings[RubikCubeSideEnum.Down] = new FaceEdges
            {
                // Front edge: from Front face bottom row (row 5, columns 3-5)
                TopEdge = new List<Cell> { new Cell(5, 3), new Cell(5, 4), new Cell(5, 5) },
                // Right edge: from Right face bottom row (row 5, columns 6-8)
                RightEdge = new List<Cell> { new Cell(5, 6), new Cell(5, 7), new Cell(5, 8) },
                // Back edge: from Bottom face bottom row (row 5, columns 9-11)
                BottomEdge = new List<Cell> { new Cell(5, 9), new Cell(5, 10), new Cell(5, 11) },
                // Left edge: from Left face bottom row (row 5, columns 0-2)
                LeftEdge = new List<Cell> { new Cell(5, 0), new Cell(5, 1), new Cell(5, 2) }
            };

            return mappings;
        }
    }
}
