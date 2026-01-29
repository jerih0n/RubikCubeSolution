using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using RubikCubeSolution.Web.Models;

namespace RubikCubeSolution.Web.Services
{
    public class RubikCubeProcessor : IRubikCubeProcessor
    {
        private readonly RubikCube _rubikCube;

        public RubikCubeProcessor(RubikCube rubikCube)
        {
            _rubikCube = rubikCube;
        }

        public MatrixCellFillEnum[,] GetMatrix()
        {
            return _rubikCube.Matrix;
        }

        public int[][] Rotate(RotationRequest request)
        {
            switch (request.Side)
            {
                case RubikCubeSideEnum.Front:
                    _rubikCube.RotateFront(request.Clockwise);
                    break;

                case RubikCubeSideEnum.Right:
                    _rubikCube.RotateRight(request.Clockwise);
                    break;

                case RubikCubeSideEnum.Upper:
                    _rubikCube.RotateUpper(request.Clockwise);
                    break;

                case RubikCubeSideEnum.Bottom:
                    _rubikCube.RotateBottom(request.Clockwise);
                    break;

                case RubikCubeSideEnum.Left:
                    _rubikCube.RotateLeft(request.Clockwise);
                    break;

                case RubikCubeSideEnum.Down:
                    _rubikCube.RotateDown(request.Clockwise);
                    break;

                default:
                    throw new ArgumentException($"Unknown rotation side: {request.Side}");
            }

            return ConvertMatrixToArray(_rubikCube.Matrix);
        }

        public int[][] Reset()
        {
            _rubikCube.Reset();
            return ConvertMatrixToArray(_rubikCube.Matrix);
        }

        private int[][] ConvertMatrixToArray(MatrixCellFillEnum[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var result = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = (int)matrix[i, j];
                }
            }

            return result;
        }
    }
}