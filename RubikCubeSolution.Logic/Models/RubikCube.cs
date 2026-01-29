using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Services;
using System.Text;

namespace RubikCubeSolution.Logic.Models
{
    public class RubikCube
    {
        private MatrixCellFillEnum[,] _matrix = new MatrixCellFillEnum[MatrixConstants.ROWS_COUNT, MatrixConstants.COLUMNS_COUNT];
        private readonly List<RubikCubeSide> _rubikCubeSides;
        private readonly RubikCubeRotationService _rotationService;

        public RubikCube()
        {
            _rubikCubeSides = BuildSides();
            _matrix = InitRubikCube();
            _rotationService = new RubikCubeRotationService();
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

        public MatrixCellFillEnum[,] Matrix => _matrix;

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

        public void Rotate(RubikCubeSideEnum side, bool clockwise)
        {
            _rotationService.Rotate(_matrix, side, clockwise);
        }

        public void RotateFront(bool clockwise = true)
        {
            _rotationService.RotateFront(_matrix, clockwise);
        }

        public void RotateRight(bool clockwise = true)
        {
            _rotationService.RotateRight(_matrix, clockwise);
        }

        public void RotateUpper(bool clockwise = true)
        {
            _rotationService.RotateUpper(_matrix, clockwise);
        }

        public void RotateBottom(bool clockwise = true)
        {
            _rotationService.RotateBottom(_matrix, clockwise);
        }

        public void RotateLeft(bool clockwise = true)
        {
            _rotationService.RotateLeft(_matrix, clockwise);
        }

        public void RotateDown(bool clockwise = true)
        {
            _rotationService.RotateDown(_matrix, clockwise);
        }

        public void Reset()
        {
            _matrix = InitRubikCube();
        }
    }
}
