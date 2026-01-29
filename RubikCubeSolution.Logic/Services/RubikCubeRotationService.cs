using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Helpers;

namespace RubikCubeSolution.Logic.Services
{
    public class RubikCubeRotationService
    {
        public void Rotate(MatrixCellFillEnum[,] matrix, RubikCubeSideEnum side, bool clockwise)
        {
            RotationHelpers.RotateLayer(matrix, side, clockwise);
        }

        public void RotateFront(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Front, clockwise);
        }

        public void RotateRight(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Right, clockwise);
        }

        public void RotateUpper(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Upper, clockwise);
        }

        public void RotateBottom(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Bottom, clockwise);
        }

        public void RotateLeft(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Left, clockwise);
        }

        public void RotateDown(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Down, clockwise);
        }

    }
}