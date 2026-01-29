using RubikCube.Playground.Configuration;
using RubikCube.Playground.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubikCube.Playground
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