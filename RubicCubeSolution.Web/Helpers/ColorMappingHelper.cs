using RubikCubeSolution.Logic.Enums;

namespace RubikCubeSolution.Web.Helpers
{
    public static class ColorMappingHelper
    {
        public static string GetHexColor(MatrixCellFillEnum color)
        {
            return color switch
            {
                MatrixCellFillEnum.Orange => "#FF6B35",
                MatrixCellFillEnum.White => "#FFFFFF",
                MatrixCellFillEnum.Green => "#00A651",
                MatrixCellFillEnum.Yellow => "#FFD700",
                MatrixCellFillEnum.Red => "#DC143C",
                MatrixCellFillEnum.Blue => "#0066CC",
                MatrixCellFillEnum.None => "transparent",
                _ => "#000000"
            };
        }
    }
}
