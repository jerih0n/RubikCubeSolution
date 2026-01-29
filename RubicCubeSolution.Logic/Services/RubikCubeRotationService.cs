using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Helpers;
using System;

namespace RubikCubeSolution.Logic.Services
{
    public class RubikCubeRotationService
    {
        private readonly EdgeMappingConfiguration _edgeMapping;

        public RubikCubeRotationService()
        {
            _edgeMapping = new EdgeMappingConfiguration();
        }

        /// <summary>
        /// Main rotation method - rotates a face clockwise or counter-clockwise
        /// </summary>
        public void Rotate(MatrixCellFillEnum[,] matrix, RubikCubeSideEnum side, bool clockwise)
        {
            var sideConfig = GetSideConfig(side);
            var edges = _edgeMapping.GetEdgesForFace(side);

            if (clockwise)
            {
                // Rotate the face clockwise
                RotationHelpers.RotateFaceClockwise(
                    matrix,
                    sideConfig.StartRowIndex,
                    sideConfig.EndRowIndex,
                    sideConfig.StartColumnIndex,
                    sideConfig.EndColumnIndex);

                // Rotate the edges clockwise
                RotationHelpers.RotateEdgesClockwise(
                    matrix,
                    edges.TopEdge,
                    edges.RightEdge,
                    edges.BottomEdge,
                    edges.LeftEdge);
            }
            else
            {
                // Rotate the face counter-clockwise
                RotationHelpers.RotateFaceCounterClockwise(
                    matrix,
                    sideConfig.StartRowIndex,
                    sideConfig.EndRowIndex,
                    sideConfig.StartColumnIndex,
                    sideConfig.EndColumnIndex);

                // Rotate the edges counter-clockwise
                RotationHelpers.RotateEdgesCounterClockwise(
                    matrix,
                    edges.TopEdge,
                    edges.RightEdge,
                    edges.BottomEdge,
                    edges.LeftEdge);
            }
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

        private RubikCubeSideLocationConfig GetSideConfig(RubikCubeSideEnum side)
        {
            return side switch
            {
                RubikCubeSideEnum.Left => SideLocationConstants.LEFT_SIDE_CONFIG,
                RubikCubeSideEnum.Upper => SideLocationConstants.UPPER_SIDE_CONFIG,
                RubikCubeSideEnum.Front => SideLocationConstants.FRONT_SIDE_CONFIG,
                RubikCubeSideEnum.Down => SideLocationConstants.DOWN_SIDE_CONFIG,
                RubikCubeSideEnum.Right => SideLocationConstants.RIGHT_SIDE_CONFIG,
                RubikCubeSideEnum.Bottom => SideLocationConstants.BOTTOM_SIDE_CONFIG,
                _ => throw new ArgumentException($"Unknown side: {side}")
            };
        }
    }
}
