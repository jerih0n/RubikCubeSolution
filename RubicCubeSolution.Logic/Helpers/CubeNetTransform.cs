using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using System;
using System.Collections.Generic;

namespace RubikCubeSolution.Logic.Helpers
{
    /// <summary>
    /// Maps the 2D cube net (matrix) to a 3D sticker model and provides exact 90° rotations.
    /// Sticker identity is (CubiePosition P, FaceNormal N). This avoids corner-collisions.
    /// </summary>
    internal static class CubeNetTransform
    {
        internal readonly record struct Vec3i(int X, int Y, int Z);

        internal readonly record struct StickerKey(Vec3i P, Vec3i N);

        private static readonly Dictionary<Cell, StickerKey> _cellToSticker;
        private static readonly Dictionary<StickerKey, Cell> _stickerToCell;

        static CubeNetTransform()
        {
            (_cellToSticker, _stickerToCell) = BuildMaps();
        }

        internal static StickerKey GetSticker(Cell cell) => _cellToSticker[cell];

        internal static Cell GetCell(StickerKey sticker) => _stickerToCell[sticker];

        internal static Vec3i GetFaceNormal(RubikCubeSideEnum face) => face switch
        {
            RubikCubeSideEnum.Front => new Vec3i(0, 0, 1),
            RubikCubeSideEnum.Bottom => new Vec3i(0, 0, -1), // Back (Blue) in this net
            RubikCubeSideEnum.Upper => new Vec3i(0, 1, 0),
            RubikCubeSideEnum.Down => new Vec3i(0, -1, 0),
            RubikCubeSideEnum.Right => new Vec3i(1, 0, 0),
            RubikCubeSideEnum.Left => new Vec3i(-1, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(face), face, "Unknown face")
        };

        internal static bool IsInRotatedLayer(StickerKey sticker, RubikCubeSideEnum face)
        {
            var n = GetFaceNormal(face);
            if (n.X != 0) return sticker.P.X == n.X;
            if (n.Y != 0) return sticker.P.Y == n.Y;
            return sticker.P.Z == n.Z;
        }

        /// <summary>
        /// Rotates a sticker around the given face's outward normal.
        /// Clockwise is as viewed from outside that face (standard cube notation).
        /// </summary>
        internal static StickerKey RotateSticker(StickerKey sticker, RubikCubeSideEnum face, bool clockwise)
        {
            var nFace = GetFaceNormal(face);

            // clockwise => -90° around face normal.
            // If face normal is negative along the axis, rotation around +axis flips sign:
            // Rot_{s*u}(theta) == Rot_u(s*theta).
            var dir = clockwise ? -1 : 1; // -1 => -90°, +1 => +90°

            if (nFace.X != 0)
            {
                var dirAroundPosAxis = dir * nFace.X;
                return new StickerKey(
                    RotateAroundX(sticker.P, dirAroundPosAxis),
                    RotateAroundX(sticker.N, dirAroundPosAxis));
            }

            if (nFace.Y != 0)
            {
                var dirAroundPosAxis = dir * nFace.Y;
                return new StickerKey(
                    RotateAroundY(sticker.P, dirAroundPosAxis),
                    RotateAroundY(sticker.N, dirAroundPosAxis));
            }

            var dirAroundPosZ = dir * nFace.Z;
            return new StickerKey(
                RotateAroundZ(sticker.P, dirAroundPosZ),
                RotateAroundZ(sticker.N, dirAroundPosZ));
        }

        private static Vec3i RotateAroundX(Vec3i v, int dirAroundPosAxis)
        {
            // dirAroundPosAxis: -1 => -90°, +1 => +90° around +X
            return dirAroundPosAxis switch
            {
                -1 => new Vec3i(v.X, v.Z, -v.Y),
                1 => new Vec3i(v.X, -v.Z, v.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(dirAroundPosAxis), dirAroundPosAxis, "Expected -1 or +1")
            };
        }

        private static Vec3i RotateAroundY(Vec3i v, int dirAroundPosAxis)
        {
            // dirAroundPosAxis: -1 => -90°, +1 => +90° around +Y
            return dirAroundPosAxis switch
            {
                -1 => new Vec3i(-v.Z, v.Y, v.X),
                1 => new Vec3i(v.Z, v.Y, -v.X),
                _ => throw new ArgumentOutOfRangeException(nameof(dirAroundPosAxis), dirAroundPosAxis, "Expected -1 or +1")
            };
        }

        private static Vec3i RotateAroundZ(Vec3i v, int dirAroundPosAxis)
        {
            // dirAroundPosAxis: -1 => -90°, +1 => +90° around +Z
            return dirAroundPosAxis switch
            {
                -1 => new Vec3i(v.Y, -v.X, v.Z),
                1 => new Vec3i(-v.Y, v.X, v.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(dirAroundPosAxis), dirAroundPosAxis, "Expected -1 or +1")
            };
        }

        private static (Dictionary<Cell, StickerKey> cellToSticker, Dictionary<StickerKey, Cell> stickerToCell) BuildMaps()
        {
            var cellToSticker = new Dictionary<Cell, StickerKey>();
            var stickerToCell = new Dictionary<StickerKey, Cell>();

            void AddFace(RubikCubeSideEnum face, RubikCubeSideLocationConfig cfg)
            {
                var n = GetFaceNormal(face);

                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        var cell = new Cell(cfg.StartRowIndex + r, cfg.StartColumnIndex + c);
                        var sticker = new StickerKey(GetCubiePosition(face, r, c), n);

                        cellToSticker[cell] = sticker;
                        stickerToCell[sticker] = cell;
                    }
                }
            }

            AddFace(RubikCubeSideEnum.Left, SideLocationConstants.LEFT_SIDE_CONFIG);
            AddFace(RubikCubeSideEnum.Upper, SideLocationConstants.UPPER_SIDE_CONFIG);
            AddFace(RubikCubeSideEnum.Front, SideLocationConstants.FRONT_SIDE_CONFIG);
            AddFace(RubikCubeSideEnum.Down, SideLocationConstants.DOWN_SIDE_CONFIG);
            AddFace(RubikCubeSideEnum.Right, SideLocationConstants.RIGHT_SIDE_CONFIG);
            AddFace(RubikCubeSideEnum.Bottom, SideLocationConstants.BOTTOM_SIDE_CONFIG);

            return (cellToSticker, stickerToCell);
        }

        private static Vec3i GetCubiePosition(RubikCubeSideEnum face, int localRow, int localCol)
        {
            // Coordinate system:
            // - X: right (+) / left (-)
            // - Y: up (+) / down (-)
            // - Z: front (+) / back (-)
            //
            // The 2D net uses these face orientations:
            // - Front: normal +Z
            // - Back (enum Bottom): normal -Z, flipped horizontally in the net (col->x reversed)
            // - Upper: normal +Y, bottom row touches Front top row
            // - Down: normal -Y, top row touches Front bottom row
            // - Right: normal +X, left col touches Front right col
            // - Left: normal -X, right col touches Front left col
            return face switch
            {
                RubikCubeSideEnum.Front => new Vec3i(
                    X: -1 + localCol,
                    Y: 1 - localRow,
                    Z: 1),

                RubikCubeSideEnum.Bottom => new Vec3i(
                    X: 1 - localCol,     // flipped horizontally
                    Y: 1 - localRow,
                    Z: -1),

                RubikCubeSideEnum.Upper => new Vec3i(
                    X: -1 + localCol,
                    Y: 1,
                    Z: -1 + localRow),

                RubikCubeSideEnum.Down => new Vec3i(
                    X: -1 + localCol,
                    Y: -1,
                    Z: 1 - localRow),

                RubikCubeSideEnum.Right => new Vec3i(
                    X: 1,
                    Y: 1 - localRow,
                    Z: 1 - localCol),

                RubikCubeSideEnum.Left => new Vec3i(
                    X: -1,
                    Y: 1 - localRow,
                    Z: -1 + localCol),

                _ => throw new ArgumentOutOfRangeException(nameof(face), face, "Unknown face")
            };
        }
    }
}

