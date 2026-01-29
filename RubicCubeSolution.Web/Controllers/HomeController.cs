using Microsoft.AspNetCore.Mvc;
using RubikCubeSolution.Logic.Models;
using RubikCubeSolution.Web.Models;
using RubikCubeSolution.Web.Services;
using System.Diagnostics;

namespace RubikCubeSolution.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRubikCubeProcessor _rubikCubeProcessor;

        public HomeController(ILogger<HomeController> logger, IRubikCubeProcessor rubikCubeProcessor)
        {
            _logger = logger;
            _rubikCubeProcessor = rubikCubeProcessor;
        }

        public IActionResult Index()
        {
            var matrix = _rubikCubeProcessor.GetMatrix();
            return View(matrix);
        }

        [HttpPost]
        public IActionResult Rotate([FromBody] RotationRequest request)
        {
            try
            {
                var matrix = _rubikCubeProcessor.Rotate(request);
                return Json(new { matrix = matrix });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid rotation request");
                return BadRequest(new { error = ex.Message });
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
                var matrix = _rubikCubeProcessor.Reset();
                return Json(new { matrix = matrix });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting cube");
                return StatusCode(500, new { error = ex.Message });
            }
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
