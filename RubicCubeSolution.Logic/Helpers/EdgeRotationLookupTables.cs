using RubikCubeSolution.Logic.Enums;
using System.Collections.Generic;

namespace RubikCubeSolution.Logic.Helpers
{
    /// <summary>
    /// Pre-computed lookup tables for edge rotations
    /// Each entry maps: (sourceEdgeIndex) -> (targetEdge, targetIndex)
    /// </summary>
    public static class EdgeRotationLookupTables
    {
        // Edge indices: 0=Top, 1=Right, 2=Bottom, 3=Left
        public enum EdgeIndex { Top = 0, Right = 1, Bottom = 2, Left = 3 }

        public struct EdgeMapping
        {
            public EdgeIndex TargetEdge;
            public int TargetIndex;

            public EdgeMapping(EdgeIndex targetEdge, int targetIndex)
            {
                TargetEdge = targetEdge;
                TargetIndex = targetIndex;
            }
        }

        // Lookup table: [face][clockwise][sourceEdge][sourceIndex] -> EdgeMapping
        // Structure: EdgeMapping[4][3] where first index is edge (Top=0, Right=1, Bottom=2, Left=3), second is cell index (0,1,2)
        private static readonly Dictionary<RubikCubeSideEnum, Dictionary<bool, EdgeMapping[][]>> _lookupTables;

        static EdgeRotationLookupTables()
        {
            _lookupTables = BuildLookupTables();
        }

        public static EdgeMapping GetMapping(RubikCubeSideEnum face, bool clockwise, EdgeIndex sourceEdge, int sourceIndex)
        {
            return _lookupTables[face][clockwise][(int)sourceEdge][sourceIndex];
        }

        private static Dictionary<RubikCubeSideEnum, Dictionary<bool, EdgeMapping[][]>> BuildLookupTables()
        {
            var tables = new Dictionary<RubikCubeSideEnum, Dictionary<bool, EdgeMapping[][]>>();

            // Front Face (F)
            tables[RubikCubeSideEnum.Front] = new Dictionary<bool, EdgeMapping[][]>
            {
                // Clockwise: Top → Right → Bottom → Left → Top
                [true] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → RightEdge[0,1,2]
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 0), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 2) },
                    // RightEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) },
                    // BottomEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // LeftEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) }
                },
                // Counter-clockwise: Top → Left → Bottom → Right → Top
                [false] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // RightEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) },
                    // BottomEdge[0,1,2] → RightEdge[0,1,2]
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 0), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 2) },
                    // LeftEdge[0,1,2] → BottomEdge[0,1,2]
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 0), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 2) }
                }
            };

            // Right Face (R) - All edges vertical
            tables[RubikCubeSideEnum.Right] = new Dictionary<bool, EdgeMapping[][]>
            {
                // Clockwise: Top → Right → Bottom → Left → Top
                [true] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // RightEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) },
                    // BottomEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // LeftEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) }
                },
                // Counter-clockwise: Top → Left → Bottom → Right → Top
                [false] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → LeftEdge[0,1,2] (direct)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 0), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 2) },
                    // RightEdge[0,1,2] → TopEdge[0,1,2] (direct)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 0), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 2) },
                    // BottomEdge[0,1,2] → RightEdge[2,1,0] (reversed) - KEEP THIS AS IS
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // LeftEdge[0,1,2] → BottomEdge[0,1,2] (direct)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 0), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 2) }
                }
            };

            // Upper Face (U) - All edges horizontal
            tables[RubikCubeSideEnum.Upper] = new Dictionary<bool, EdgeMapping[][]>
            {
                // Clockwise: Top → Right → Bottom → Left → Top
                [true] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // RightEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) },
                    // BottomEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // LeftEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) }
                },
                // Counter-clockwise: Top → Left → Bottom → Right → Top
                [false] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // RightEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) },
                    // BottomEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // LeftEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) }
                }
            };

            // Back Face (B) - Reversed direction
            tables[RubikCubeSideEnum.Bottom] = new Dictionary<bool, EdgeMapping[][]>
            {
                // Clockwise (reversed): Top → Left → Bottom → Right → Top
                [true] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // RightEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) },
                    // BottomEdge[0,1,2] → RightEdge[0,1,2] (direct)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 0), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 2) },
                    // LeftEdge[0,1,2] → BottomEdge[0,1,2] (direct)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 0), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 2) }
                },
                // Counter-clockwise: Top → Right → Bottom → Left → Top
                [false] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → RightEdge[0,1,2] (direct)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 0), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 2) },
                    // RightEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) },
                    // BottomEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // LeftEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) }
                }
            };

            // Left Face (L) - All edges vertical
            tables[RubikCubeSideEnum.Left] = new Dictionary<bool, EdgeMapping[][]>
            {
                // Clockwise: Top → Right → Bottom → Left → Top
                [true] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // RightEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) },
                    // BottomEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // LeftEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) }
                },
                // Counter-clockwise: Top → Left → Bottom → Right → Top
                [false] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // RightEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) },
                    // BottomEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // LeftEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) }
                }
            };

            // Down Face (D) - All edges horizontal
            tables[RubikCubeSideEnum.Down] = new Dictionary<bool, EdgeMapping[][]>
            {
                // Clockwise: Top → Right → Bottom → Left → Top
                [true] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // RightEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) },
                    // BottomEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // LeftEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) }
                },
                // Counter-clockwise: Top → Left → Bottom → Right → Top
                [false] = new EdgeMapping[][]
                {
                    // TopEdge[0,1,2] → LeftEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Left, 2), new EdgeMapping(EdgeIndex.Left, 1), new EdgeMapping(EdgeIndex.Left, 0) },
                    // RightEdge[0,1,2] → TopEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Top, 2), new EdgeMapping(EdgeIndex.Top, 1), new EdgeMapping(EdgeIndex.Top, 0) },
                    // BottomEdge[0,1,2] → RightEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Right, 2), new EdgeMapping(EdgeIndex.Right, 1), new EdgeMapping(EdgeIndex.Right, 0) },
                    // LeftEdge[0,1,2] → BottomEdge[2,1,0] (reversed)
                    new EdgeMapping[] { new EdgeMapping(EdgeIndex.Bottom, 2), new EdgeMapping(EdgeIndex.Bottom, 1), new EdgeMapping(EdgeIndex.Bottom, 0) }
                }
            };

            return tables;
        }
    }
}
