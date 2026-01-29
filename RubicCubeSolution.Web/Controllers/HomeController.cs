using Microsoft.AspNetCore.Mvc;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Logic.Models;
using RubikCubeSolution.Web.Models;
using System;
using System.Diagnostics;

namespace RubikCubeSolution.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RubikCube _rubikCube;

        public HomeController(ILogger<HomeController> logger, RubikCube rubikCube)
        {
            _logger = logger;
            _rubikCube = rubikCube;
        }

        public IActionResult Index()
        {
            var matrix = _rubikCube.Matrix;
            return View(matrix);
        }

        [HttpPost]
        public IActionResult Rotate([FromBody] RotationRequest request)
        {
            try
            {
                var instruction = request.Instruction?.ToUpper();
                var clockwise = !instruction?.EndsWith("'") ?? true;

                // Remove the prime symbol if present
                var baseInstruction = instruction?.Replace("'", "").Trim();

                switch (baseInstruction)
                {
                    case "F":
                        _rubikCube.RotateFront(clockwise);
                        break;
                    case "R":
                        _rubikCube.RotateRight(clockwise);
                        break;
                    case "U":
                        _rubikCube.RotateUpper(clockwise);
                        break;
                    case "B":
                        _rubikCube.RotateBottom(clockwise);
                        break;
                    case "L":
                        _rubikCube.RotateLeft(clockwise);
                        break;
                    case "D":
                        _rubikCube.RotateDown(clockwise);
                        break;
                    default:
                        return BadRequest(new { error = $"Unknown rotation instruction: {request.Instruction}" });
                }

                var matrix = _rubikCube.Matrix;
                return Json(new { matrix = ConvertMatrixToArray(matrix) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing rotation");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Reset()
        {
            try
            {
                _rubikCube.Reset();
                var matrix = _rubikCube.Matrix;
                return Json(new { matrix = ConvertMatrixToArray(matrix) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting cube");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private int[][] ConvertMatrixToArray(MatrixCellFillEnum[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var result = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = (int)matrix[i, j];
                }
            }

            return result;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
