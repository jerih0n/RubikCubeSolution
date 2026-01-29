using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Logic.Configuration
{
    public class FaceEdges
    {
        public List<Cell> TopEdge { get; set; } = new List<Cell>();
        public List<Cell> RightEdge { get; set; } = new List<Cell>();
        public List<Cell> BottomEdge { get; set; } = new List<Cell>();
        public List<Cell> LeftEdge { get; set; } = new List<Cell>();
    }
}