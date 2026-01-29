using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Helpers;

namespace RubikCubeSolution.Logic.Services
{
    public class RubikCubeRotationService
    {
        /// <summary>
        /// Main rotation method - rotates a face clockwise or counter-clockwise
        /// </summary>
        public void Rotate(MatrixCellFillEnum[,] matrix, RubikCubeSideEnum side, bool clockwise)
        {
            RotationHelpers.RotateLayer(matrix, side, clockwise);
        }

        /// <summary>
        /// Rotate Front face (F)
        /// </summary>
        public void RotateFront(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Front, clockwise);
        }

        /// <summary>
        /// Rotate Right face (R)
        /// </summary>
        public void RotateRight(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Right, clockwise);
        }

        /// <summary>
        /// Rotate Upper face (U)
        /// </summary>
        public void RotateUpper(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Upper, clockwise);
        }

        /// <summary>
        /// Rotate Bottom/Back face (B)
        /// Note: Back face edges rotate in reverse order: Top → Left → Bottom → Right → Top (when clockwise)
        /// </summary>
        public void RotateBottom(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Bottom, clockwise);
        }

        /// <summary>
        /// Rotate Left face (L)
        /// </summary>
        public void RotateLeft(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Left, clockwise);
        }

        /// <summary>
        /// Rotate Down face (D)
        /// </summary>
        public void RotateDown(MatrixCellFillEnum[,] matrix, bool clockwise)
        {
            Rotate(matrix, RubikCubeSideEnum.Down, clockwise);
        }

        // Note: Face location configs are now handled inside CubeNetTransform/RotationHelpers.RotateLayer.
    }
}