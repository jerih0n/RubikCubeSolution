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

        /// <summary>
        /// Rotate a face by side enum
        /// </summary>
        /// <param name="side">The side to rotate</param>
        /// <param name="clockwise">True for clockwise (F, R, U, etc.), false for counter-clockwise (F', R', U', etc.)</param>
        public void Rotate(RubikCubeSideEnum side, bool clockwise)
        {
            _rotationService.Rotate(_matrix, side, clockwise);
        }

        /// <summary>
        /// Rotate Front face (F) - clockwise if true, counter-clockwise if false
        /// </summary>
        public void RotateFront(bool clockwise = true)
        {
            _rotationService.RotateFront(_matrix, clockwise);
        }

        /// <summary>
        /// Rotate Right face (R) - clockwise if true, counter-clockwise if false
        /// </summary>
        public void RotateRight(bool clockwise = true)
        {
            _rotationService.RotateRight(_matrix, clockwise);
        }

        /// <summary>
        /// Rotate Upper face (U) - clockwise if true, counter-clockwise if false
        /// </summary>
        public void RotateUpper(bool clockwise = true)
        {
            _rotationService.RotateUpper(_matrix, clockwise);
        }

        /// <summary>
        /// Rotate Bottom/Back face (B) - clockwise if true, counter-clockwise if false
        /// </summary>
        public void RotateBottom(bool clockwise = true)
        {
            _rotationService.RotateBottom(_matrix, clockwise);
        }

        /// <summary>
        /// Rotate Left face (L) - clockwise if true, counter-clockwise if false
        /// </summary>
        public void RotateLeft(bool clockwise = true)
        {
            _rotationService.RotateLeft(_matrix, clockwise);
        }

        /// <summary>
        /// Rotate Down face (D) - clockwise if true, counter-clockwise if false
        /// </summary>
        public void RotateDown(bool clockwise = true)
        {
            _rotationService.RotateDown(_matrix, clockwise);
        }
    }
}
