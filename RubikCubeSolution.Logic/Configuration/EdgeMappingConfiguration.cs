using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Logic.Configuration
{
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

            mappings[RubikCubeSideEnum.Front] = new FaceEdges
            {
                TopEdge = new List<Cell> { new Cell(2, 3), new Cell(2, 4), new Cell(2, 5) },
                RightEdge = new List<Cell> { new Cell(3, 6), new Cell(4, 6), new Cell(5, 6) },
                BottomEdge = new List<Cell> { new Cell(6, 3), new Cell(6, 4), new Cell(6, 5) },
                LeftEdge = new List<Cell> { new Cell(3, 2), new Cell(4, 2), new Cell(5, 2) }
            };

            mappings[RubikCubeSideEnum.Right] = new FaceEdges
            {
                TopEdge = new List<Cell> { new Cell(0, 5), new Cell(1, 5), new Cell(2, 5) },
                RightEdge = new List<Cell> { new Cell(3, 9), new Cell(4, 9), new Cell(5, 9) },
                BottomEdge = new List<Cell> { new Cell(6, 5), new Cell(7, 5), new Cell(8, 5) },
                LeftEdge = new List<Cell> { new Cell(3, 5), new Cell(4, 5), new Cell(5, 5) }
            };

            mappings[RubikCubeSideEnum.Upper] = new FaceEdges
            {
                TopEdge = new List<Cell> { new Cell(3, 9), new Cell(3, 10), new Cell(3, 11) },
                RightEdge = new List<Cell> { new Cell(3, 6), new Cell(3, 7), new Cell(3, 8) },
                BottomEdge = new List<Cell> { new Cell(3, 3), new Cell(3, 4), new Cell(3, 5) },
                LeftEdge = new List<Cell> { new Cell(3, 0), new Cell(3, 1), new Cell(3, 2) }
            };

            mappings[RubikCubeSideEnum.Bottom] = new FaceEdges
            {
                TopEdge = new List<Cell> { new Cell(0, 3), new Cell(0, 4), new Cell(0, 5) },
                RightEdge = new List<Cell> { new Cell(3, 8), new Cell(4, 8), new Cell(5, 8) },
                BottomEdge = new List<Cell> { new Cell(8, 3), new Cell(8, 4), new Cell(8, 5) },
                LeftEdge = new List<Cell> { new Cell(3, 0), new Cell(4, 0), new Cell(5, 0) }
            };

            mappings[RubikCubeSideEnum.Left] = new FaceEdges
            {
                TopEdge = new List<Cell> { new Cell(0, 3), new Cell(1, 3), new Cell(2, 3) },
                RightEdge = new List<Cell> { new Cell(3, 3), new Cell(4, 3), new Cell(5, 3) },
                BottomEdge = new List<Cell> { new Cell(6, 3), new Cell(7, 3), new Cell(8, 3) },
                LeftEdge = new List<Cell> { new Cell(3, 11), new Cell(4, 11), new Cell(5, 11) }
            };

            mappings[RubikCubeSideEnum.Down] = new FaceEdges
            {
                TopEdge = new List<Cell> { new Cell(5, 3), new Cell(5, 4), new Cell(5, 5) },
                RightEdge = new List<Cell> { new Cell(5, 6), new Cell(5, 7), new Cell(5, 8) },
                BottomEdge = new List<Cell> { new Cell(5, 9), new Cell(5, 10), new Cell(5, 11) },
                LeftEdge = new List<Cell> { new Cell(5, 0), new Cell(5, 1), new Cell(5, 2) }
            };

            return mappings;
        }
    }
}