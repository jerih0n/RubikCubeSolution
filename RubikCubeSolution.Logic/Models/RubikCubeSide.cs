using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Enums;

namespace RubikCubeSolution.Logic.Models
{
    internal class RubikCubeSide
    {
        public RubikCubeSide(RubikCubeSideLocationConfig rubikCubeSideConfiguration,
            RubikCubeSideEnum type,
            MatrixCellFillEnum colorFill)
        {
            SideConfig = rubikCubeSideConfiguration;
            Type = type;
            Color = colorFill;
            Cells = BuildSideLocation();
        }

        public RubikCubeSideLocationConfig SideConfig { get; }
        public RubikCubeSideEnum Type { get; }

        public MatrixCellFillEnum Color { get; }

        public List<Cell> Cells { get; }

        public MatrixCellFillEnum[,] RotateClockwise(MatrixCellFillEnum[,] matrixState)
        {
            return matrixState;
        }

        public MatrixCellFillEnum[,] RotateCounterClockwise(MatrixCellFillEnum[,] matrixState)
        {
            return matrixState;
        }

        private List<Cell> BuildSideLocation()
        {
            List<Cell> cellsResult = new List<Cell>();
            for (int i = SideConfig.StartRowIndex; i <= SideConfig.EndRowIndex; i++)
            {
                for (int j = SideConfig.StartColumnIndex; j <= SideConfig.EndColumnIndex; j++)
                {
                    cellsResult.Add(new Cell(i, j));
                }
            }

            return cellsResult;
        }
    }
}
