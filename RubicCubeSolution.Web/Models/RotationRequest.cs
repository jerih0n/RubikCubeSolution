using RubikCubeSolution.Logic.Enums;

namespace RubikCubeSolution.Web.Models
{
    public class RotationRequest
    {
        public RubikCubeSideEnum Side { get; set; }
        public bool Clockwise { get; set; } = true;
    }
}
