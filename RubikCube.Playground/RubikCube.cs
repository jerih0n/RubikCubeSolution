using RubikCube.Playground.Configuration;
using RubikCube.Playground.Constants;
using RubikCube.Playground.Enums;
using System.Text;

namespace RubikCube.Playground
{
    public class RubikCube
    {
        private MatrixCellFillEnum[,] _matrix = new MatrixCellFillEnum[MatrixConstants.ROWS_COUNT, MatrixConstants.COLUMNS_COUNT];
        private readonly List<RubikCubeSide> _rubikCubeSides;

        public RubikCube()
        {
            _rubikCubeSides = BuildSides();
            _matrix = InitRubikCube();
        }

        private MatrixCellFillEnum[,] InitRubikCube()
        {
            for (int i = 0; i < MatrixConstants.ROWS_COUNT; i++)
            {
                for (int j = 0; j < MatrixConstants.COLUMNS_COUNT; j++)
                {
                    _matrix[i, j] = MatrixCellFillEnum.None;
                }
            }

            foreach (var side in _rubikCubeSides)
            {
                var cells = side.Cells;
                var color = side.Color;

                foreach (var cell in cells)
                {
                    _matrix[cell.Row, cell.Col] = color;
                }
            }
            return _matrix;
        }

        public MatrixCellFillEnum[,] Matrix
        { get { return _matrix; } }

        public string PrintMatrix()
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < MatrixConstants.ROWS_COUNT; i++)
            {
                for (int j = 0; j < MatrixConstants.COLUMNS_COUNT; j++)
                {
                    stringBuilder.Append(_matrix[i, j].ToString() + $"({i}, {j})");
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        private List<RubikCubeSide> BuildSides()
        {
            var result = new List<RubikCubeSide>()
            {
                BuildRubikCubeSide(SideLocationConstants.LEFT_SIDE_CONFIG, RubikCubeSideEnum.Left, MatrixCellFillEnum.Orange),
                BuildRubikCubeSide(SideLocationConstants.UPPER_SIDE_CONFIG, RubikCubeSideEnum.Upper, MatrixCellFillEnum.White),
                BuildRubikCubeSide(SideLocationConstants.FRONT_SIDE_CONFIG, RubikCubeSideEnum.Front, MatrixCellFillEnum.Green),
                BuildRubikCubeSide(SideLocationConstants.DOWN_SIDE_CONFIG, RubikCubeSideEnum.Down, MatrixCellFillEnum.Yellow),
                BuildRubikCubeSide(SideLocationConstants.RIGHT_SIDE_CONFIG, RubikCubeSideEnum.Right, MatrixCellFillEnum.Red),
                BuildRubikCubeSide(SideLocationConstants.BOTTOM_SIDE_CONFIG, RubikCubeSideEnum.Bottom, MatrixCellFillEnum.Blue),
            };

            return result;
        }

        private RubikCubeSide BuildRubikCubeSide(RubikCubeSideLocationConfig sideConfig, RubikCubeSideEnum type, MatrixCellFillEnum matrixCellFillEnum)
        {
            return new RubikCubeSide(sideConfig, type, matrixCellFillEnum);
        }
    }
}