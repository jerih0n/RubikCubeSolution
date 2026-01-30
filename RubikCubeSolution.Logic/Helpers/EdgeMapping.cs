using RubikCubeSolution.Logic.Enums;

namespace RubikCubeSolution.Logic.Helpers
{
    public readonly struct EdgeMapping
    {
        public EdgeIndex TargetEdge { get; }
        public int TargetIndex { get; }

        public EdgeMapping(EdgeIndex targetEdge, int targetIndex)
        {
            TargetEdge = targetEdge;
            TargetIndex = targetIndex;
        }
    }
}