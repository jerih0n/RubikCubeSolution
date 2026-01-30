using RubikCubeSolution.Logic.Configuration;
using RubikCubeSolution.Logic.Constants;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;

namespace RubikCubeSolution.Logic.Helpers
{
    internal static class CubeNetTransform
    {
        private static readonly Dictionary<Cell, StickerKey> _cellToSticker;
        private static readonly Dictionary<StickerKey, Cell> _stickerToCell;

        static CubeNetTransform()
        {
            (_cellToSticker, _stickerToCell) = BuildMaps();
        }

        internal static StickerKey GetSticker(Cell cell) => _cellToSticker[cell];

        internal static Cell GetCell(StickerKey sticker) => _stickerToCell[sticker];

        internal static Vector GetFaceNormal(RubikCubeSideEnum face) => face switch
        {
            RubikCubeSideEnum.Front => new Vector(0, 0, 1),
            RubikCubeSideEnum.Bottom => new Vector(0, 0, -1),
            RubikCubeSideEnum.Upper => new Vector(0, 1, 0),
            RubikCubeSideEnum.Down => new Vector(0, -1, 0),
            RubikCubeSideEnum.Right => new Vector(1, 0, 0),
            RubikCubeSideEnum.Left => new Vector(-1, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(face), face, "Unknown face")
        };

        internal static bool IsInRotatedLayer(StickerKey sticker, RubikCubeSideEnum face)
        {
            var faceNormal = GetFaceNormal(face);
            if (faceNormal.X != 0) return sticker.Position.X == faceNormal.X;
            if (faceNormal.Y != 0) return sticker.Position.Y == faceNormal.Y;
            return sticker.Position.Z == faceNormal.Z;
        }

        internal static StickerKey RotateSticker(StickerKey sticker, RubikCubeSideEnum face, bool clockwise)
        {
            var faceNormal = GetFaceNormal(face);
            var direction = clockwise ? -1 : 1;

            if (faceNormal.X != 0)
            {
                var directionAroundAxis = direction * faceNormal.X;
                return new StickerKey(
                    RotateAroundX(sticker.Position, directionAroundAxis),
                    RotateAroundX(sticker.FaceNormal, directionAroundAxis));
            }

            if (faceNormal.Y != 0)
            {
                var directionAroundAxis = direction * faceNormal.Y;
                return new StickerKey(
                    RotateAroundY(sticker.Position, directionAroundAxis),
                    RotateAroundY(sticker.FaceNormal, directionAroundAxis));
            }

            var directionAroundZ = direction * faceNormal.Z;
            return new StickerKey(
                RotateAroundZ(sticker.Position, directionAroundZ),
                RotateAroundZ(sticker.FaceNormal, directionAroundZ));
        }

        private static Vector RotateAroundX(Vector vector, int directionAroundAxis)
        {
            return directionAroundAxis switch
            {
                -1 => new Vector(vector.X, vector.Z, -vector.Y),
                1 => new Vector(vector.X, -vector.Z, vector.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(directionAroundAxis), directionAroundAxis, "Expected -1 or +1")
            };
        }

        private static Vector RotateAroundY(Vector vector, int directionAroundAxis)
        {
            return directionAroundAxis switch
            {
                -1 => new Vector(-vector.Z, vector.Y, vector.X),
                1 => new Vector(vector.Z, vector.Y, -vector.X),
                _ => throw new ArgumentOutOfRangeException(nameof(directionAroundAxis), directionAroundAxis, "Expected -1 or +1")
            };
        }

        private static Vector RotateAroundZ(Vector vector, int directionAroundAxis)
        {
            return directionAroundAxis switch
            {
                -1 => new Vector(vector.Y, -vector.X, vector.Z),
                1 => new Vector(-vector.Y, vector.X, vector.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(directionAroundAxis), directionAroundAxis, "Expected -1 or +1")
            };
        }

        private static (Dictionary<Cell, StickerKey> cellToSticker, Dictionary<StickerKey, Cell> stickerToCell) BuildMaps()
        {
            var cellToSticker = new Dictionary<Cell, StickerKey>();
            var stickerToCell = new Dictionary<StickerKey, Cell>();

            void AddFace(RubikCubeSideEnum face, RubikCubeSideLocationConfig cfg)
            {
                var faceNormal = GetFaceNormal(face);

                for (int row = 0; row < 3; row++)
                {
                    for (int column = 0; column < 3; column++)
                    {
                        var cell = new Cell(cfg.StartRowIndex + row, cfg.StartColumnIndex + column);
                        var cubiePosition = GetCubiePosition(face, row, column);
                        var sticker = new StickerKey(cubiePosition, faceNormal);

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

        private static Vector GetCubiePosition(RubikCubeSideEnum face, int localRow, int localCol)
        {
            return face switch
            {
                RubikCubeSideEnum.Front => new Vector(
                    X: -1 + localCol,
                    Y: 1 - localRow,
                    Z: 1),

                RubikCubeSideEnum.Bottom => new Vector(
                    X: 1 - localCol,
                    Y: 1 - localRow,
                    Z: -1),

                RubikCubeSideEnum.Upper => new Vector(
                    X: -1 + localCol,
                    Y: 1,
                    Z: -1 + localRow),

                RubikCubeSideEnum.Down => new Vector(
                    X: -1 + localCol,
                    Y: -1,
                    Z: 1 - localRow),

                RubikCubeSideEnum.Right => new Vector(
                    X: 1,
                    Y: 1 - localRow,
                    Z: 1 - localCol),

                RubikCubeSideEnum.Left => new Vector(
                    X: -1,
                    Y: 1 - localRow,
                    Z: -1 + localCol),

                _ => throw new ArgumentOutOfRangeException(nameof(face), face, "Unknown face")
            };
        }
    }
}