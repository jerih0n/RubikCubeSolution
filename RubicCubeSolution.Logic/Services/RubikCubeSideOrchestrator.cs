using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Logic.Services
{
    public class RubikSideOrchestrator
    {
        public RubikSideOrchestrator()
        {
        }

        private RubikCubeSide BuildCubeSide(RubikCubeSideLocationConfig sideConfig, RubikCubeSideEnum type, MatrixCellFillEnum matrixCellFillEnum)
        {
            return new RubikCubeSide(sideConfig, type, matrixCellFillEnum);
        }
    }
}
