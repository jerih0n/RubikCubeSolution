using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Web.Models;

namespace RubikCubeSolution.Web.Services
{
    public interface IRubikCubeProcessor
    {
        MatrixCellFillEnum[,] GetMatrix();
        int[][] Rotate(RotationRequest request);
        int[][] Reset();
    }
}
