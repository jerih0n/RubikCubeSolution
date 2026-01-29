using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using System;
using System.Collections.Generic;

namespace RubikCubeSolution.Logic.Helpers
{
    public static class EdgeRotationLookupTables
    {
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

            var edgeConfig = new EdgeMappingConfiguration();

            foreach (var face in new[]
                     {
                         RubikCubeSideEnum.Front,
                         RubikCubeSideEnum.Right,
                         RubikCubeSideEnum.Upper,
                         RubikCubeSideEnum.Bottom,
                         RubikCubeSideEnum.Left,
                         RubikCubeSideEnum.Down
                     })
            {
                var edges = edgeConfig.GetEdgesForFace(face);
                var edgeLists = new[] { edges.TopEdge, edges.RightEdge, edges.BottomEdge, edges.LeftEdge };

                tables[face] = new Dictionary<bool, EdgeMapping[][]>
                {
                    [true] = BuildFaceTable(face, true, edgeLists),
                    [false] = BuildFaceTable(face, false, edgeLists),
                };
            }

            return tables;
        }

        private static EdgeMapping[][] BuildFaceTable(RubikCubeSideEnum face, bool clockwise, List<Cell>[] edgeLists)
        {
            var result = new EdgeMapping[4][];

            for (int e = 0; e < 4; e++)
            {
                result[e] = new EdgeMapping[3];
                for (int i = 0; i < 3; i++)
                {
                    var srcCell = edgeLists[e][i];
                    var srcSticker = CubeNetTransform.GetSticker(srcCell);
                    var dstSticker = CubeNetTransform.RotateSticker(srcSticker, face, clockwise);
                    var dstCell = CubeNetTransform.GetCell(dstSticker);

                    if (!TryFindEdgeIndex(edgeLists, dstCell, out var targetEdge, out var targetIndex))
                    {
                        throw new InvalidOperationException(
                            $"Generated lookup mapping failed: face={face}, clockwise={clockwise}, srcEdge={e}, srcIndex={i} " +
                            $"mapped to cell ({dstCell.Row},{dstCell.Col}) which is not in the configured edge lists.");
                    }

                    result[e][i] = new EdgeMapping((EdgeIndex)targetEdge, targetIndex);
                }
            }

            return result;
        }

        private static bool TryFindEdgeIndex(List<Cell>[] edgeLists, Cell cell, out int edgeIdx, out int index)
        {
            for (int e = 0; e < 4; e++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (edgeLists[e][i].Equals(cell))
                    {
                        edgeIdx = e;
                        index = i;
                        return true;
                    }
                }
            }

            edgeIdx = -1;
            index = -1;
            return false;
        }
    }
}
