using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using System.Collections.Generic;

namespace RubikCubeSolution.Logic.Helpers
{
    public static class RotationHelpers
    {
        /// <summary>
        /// Rotates an entire layer using the 3D sticker model (face + adjacent strips).
        /// This is the most reliable implementation because it doesn't rely on per-face reversal guesses.
        /// </summary>
        public static void RotateLayer(MatrixCellFillEnum[,] matrix, RubikCubeSideEnum face, bool clockwise)
        {
            RotateLayer<MatrixCellFillEnum>(matrix, face, clockwise);
        }

        public static void RotateLayer<T>(T[,] matrix, RubikCubeSideEnum face, bool clockwise)
        {
            // We need the original values to avoid overwrite while assigning destinations.
            var original = (T[,])matrix.Clone();

            // Rotate only stickers on the moved layer (e.g. all stickers with Z=+1 for Front).
            // Using the mapping model ensures Back face semantics are correct too.
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
            // Enumerate the 6 faces (3x3 each) using the known net layout.
            // This keeps us independent of EdgeMappingConfiguration.
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

        /// <summary>
        /// Rotates a 3x3 face clockwise in the matrix
        /// </summary>
        public static void RotateFaceClockwise(
            MatrixCellFillEnum[,] matrix,
            int startRow, int endRow,
            int startCol, int endCol)
        {
            // For a 3x3 grid, rotate clockwise by:
            // 1. Transpose (swap rows and columns)
            // 2. Reverse each row
            
            // Extract the 3x3 face
            var temp = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = matrix[startRow + i, startCol + j];
                }
            }

            // Rotate clockwise: transpose then reverse rows
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[startRow + i, startCol + j] = temp[2 - j, i];
                }
            }
        }

        /// <summary>
        /// Rotates a 3x3 face counter-clockwise in the matrix
        /// </summary>
        public static void RotateFaceCounterClockwise(
            MatrixCellFillEnum[,] matrix,
            int startRow, int endRow,
            int startCol, int endCol)
        {
            // For a 3x3 grid, rotate counter-clockwise by:
            // 1. Reverse each row
            // 2. Transpose (swap rows and columns)
            
            // Extract the 3x3 face
            var temp = new MatrixCellFillEnum[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = matrix[startRow + i, startCol + j];
                }
            }

            // Rotate counter-clockwise: reverse rows then transpose
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[startRow + i, startCol + j] = temp[j, 2 - i];
                }
            }
        }

        /// <summary>
        /// Generic edge rotation using lookup tables
        /// Uses pre-computed mappings for all face rotations
        /// </summary>
        public static void RotateEdges(
            MatrixCellFillEnum[,] matrix,
            RubikCubeSideEnum face,
            bool clockwise,
            List<Cell> topEdge,
            List<Cell> rightEdge,
            List<Cell> bottomEdge,
            List<Cell> leftEdge)
        {
            // Store current values
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

            // Apply rotations using lookup table
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
