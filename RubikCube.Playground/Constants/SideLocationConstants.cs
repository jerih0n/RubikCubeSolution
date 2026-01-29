using RubikCube.Playground.Configuration;

namespace RubikCube.Playground.Constants
{
    public static class SideLocationConstants
    {
        public static readonly RubikCubeSideLocationConfig LEFT_SIDE_CONFIG = new RubikCubeSideLocationConfig(3, 5, 0, 2);

        public static readonly RubikCubeSideLocationConfig UPPER_SIDE_CONFIG = new RubikCubeSideLocationConfig(0, 3, 3, 5);

        public static readonly RubikCubeSideLocationConfig FRONT_SIDE_CONFIG = new RubikCubeSideLocationConfig(3, 5, 3, 5);

        public static readonly RubikCubeSideLocationConfig DOWN_SIDE_CONFIG = new RubikCubeSideLocationConfig(6, 8, 3, 5);

        public static readonly RubikCubeSideLocationConfig RIGHT_SIDE_CONFIG = new RubikCubeSideLocationConfig(3, 5, 6, 8);

        public static readonly RubikCubeSideLocationConfig BOTTOM_SIDE_CONFIG = new RubikCubeSideLocationConfig(3, 5, 9, 11);
    }
}