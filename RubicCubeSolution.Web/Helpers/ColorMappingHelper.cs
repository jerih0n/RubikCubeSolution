using RubikCubeSolution.Logic.Enums;

namespace RubikCubeSolution.Web.Helpers
{
    public static class ColorMappingHelper
    {
        public static string GetHexColor(MatrixCellFillEnum color)
        {
            return color switch
            {
                MatrixCellFillEnum.Orange => "#FF6B35",  // Orange
                MatrixCellFillEnum.White => "#FFFFFF",    // White
                MatrixCellFillEnum.Green => "#00A651",    // Green
                MatrixCellFillEnum.Yellow => "#FFD700",   // Yellow/Gold
                MatrixCellFillEnum.Red => "#DC143C",      // Red/Crimson
                MatrixCellFillEnum.Blue => "#0066CC",     // Blue
                MatrixCellFillEnum.None => "transparent",  // Transparent (shouldn't be rendered)
                _ => "#000000"                            // Black as fallback
            };
        }
    }
}
