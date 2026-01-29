using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using System.Collections.Generic;

namespace RubikCubeSolution.Logic.Helpers
{
    public static class RotationHelpers
    {
        public static void RotateLayer(MatrixCellFillEnum[,] matrix, RubikCubeSideEnum face, bool clockwise)
        {
            RotateLayer<MatrixCellFillEnum>(matrix, face, clockwise);
        }

        public static void RotateLayer<T>(T[,] matrix, RubikCubeSideEnum face, bool clockwise)
        {
            var original = (T[,])matrix.Clone();

            foreach (var kvp in GetAllStickerCells())
            {
                var srcCell = kvp;
                var srcSticker = CubeNetTransform.GetSticker(srcCell);

                if (!CubeNetTransform.IsInRotatedLayer(srcSticker, face))
                    continue;

                var dstSticker = CubeNetTransform.RotateSticker(srcSticker, face, clockwise);
                var dstCell = CubeNetTransform.GetCell(dstSticker);

                matrix[dstCell.Row, dstCell.Col] = original[srcCell.Row, srcCell.Col];
            }
        }

        private static IEnumerable<Cell> GetAllStickerCells()
        {
            var faces = new[]
            {
                (RubikCubeSideEnum.Left, SideLocationConstants.LEFT_SIDE_CONFIG),
                (RubikCubeSideEnum.Upper, SideLocationConstants.UPPER_SIDE_CONFIG),
                (RubikCubeSideEnum.Front, SideLocationConstants.FRONT_SIDE_CONFIG),
                (RubikCubeSideEnum.Down, SideLocationConstants.DOWN_SIDE_CONFIG),
                (RubikCubeSideEnum.Right, SideLocationConstants.RIGHT_SIDE_CONFIG),
                (RubikCubeSideEnum.Bottom, SideLocationConstants.BOTTOM_SIDE_CONFIG),
            };

            foreach (var (_, cfg) in faces)
            {
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        yield return new Cell(cfg.StartRowIndex + r, cfg.StartColumnIndex + c);
                    }
                }
            }
        }

        public static void RotateFaceClockwise(
            MatrixCellFillEnum[,] matrix,
            int startRow, int endRow,
            int startCol, int endCol)
        {
            var temp = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = matrix[startRow + i, startCol + j];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[startRow + i, startCol + j] = temp[2 - j, i];
                }
            }
        }

        public static void RotateFaceCounterClockwise(
            MatrixCellFillEnum[,] matrix,
            int startRow, int endRow,
            int startCol, int endCol)
        {
            var temp = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = matrix[startRow + i, startCol + j];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[startRow + i, startCol + j] = temp[j, 2 - i];
                }
            }
        }

        public static void RotateEdges(
            MatrixCellFillEnum[,] matrix,
            RubikCubeSideEnum face,
            bool clockwise,
            List<Cell> topEdge,
            List<Cell> rightEdge,
            List<Cell> bottomEdge,
            List<Cell> leftEdge)
        {
            var edges = new[] { topEdge, rightEdge, bottomEdge, leftEdge };
            var values = new MatrixCellFillEnum[4][];
            
            for (int edgeIdx = 0; edgeIdx < 4; edgeIdx++)
            {
                values[edgeIdx] = new MatrixCellFillEnum[3];
                for (int i = 0; i < 3; i++)
                {
                    values[edgeIdx][i] = matrix[edges[edgeIdx][i].Row, edges[edgeIdx][i].Col];
                }
            }
            for (int sourceEdgeIdx = 0; sourceEdgeIdx < 4; sourceEdgeIdx++)
            {
                for (int sourceIndex = 0; sourceIndex < 3; sourceIndex++)
                {
                    var mapping = EdgeRotationLookupTables.GetMapping(
                        face, 
                        clockwise, 
                        (EdgeRotationLookupTables.EdgeIndex)sourceEdgeIdx, 
                        sourceIndex);

                    var targetEdge = edges[(int)mapping.TargetEdge];
                    var targetIndex = mapping.TargetIndex;
                    
                    matrix[targetEdge[targetIndex].Row, targetEdge[targetIndex].Col] = values[sourceEdgeIdx][sourceIndex];
                }
            }
        }
    }
}
