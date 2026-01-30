using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RubikCubeSolution.Logic.Enums;
using RubikCubeSolution.Web.Controllers;
using RubikCubeSolution.Web.Models;
using RubikCubeSolution.Web.Services;

namespace RubikCubeSolution.Test.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> _loggerMock;
        private Mock<IRubikCubeProcessor> _processorMock;
        private HomeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _processorMock = new Mock<IRubikCubeProcessor>();
            _controller = new HomeController(_loggerMock.Object, _processorMock.Object);
        }

        [TestMethod]
        public void Index_ReturnsViewWithMatrix()
        {
            // Arrange
            var expectedMatrix = new MatrixCellFillEnum[9, 12];
            _processorMock.Setup(p => p.GetMatrix()).Returns(expectedMatrix);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual(expectedMatrix, viewResult.Model);
            _processorMock.Verify(p => p.GetMatrix(), Times.Once);
        }

        [TestMethod]
        public void Rotate_ValidRequest_ReturnsJsonWithMatrix()
        {
            // Arrange
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Front,
                Clockwise = true
            };
            var expectedMatrix = new int[][] { new int[] { 1, 2, 3 } };
            _processorMock.Setup(p => p.Rotate(request)).Returns(expectedMatrix);

            // Act
            var result = _controller.Rotate(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
            _processorMock.Verify(p => p.Rotate(request), Times.Once);
        }

        [TestMethod]
        public void Rotate_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Front,
                Clockwise = true
            };
            _processorMock.Setup(p => p.Rotate(request))
                .Throws(new ArgumentException("Invalid rotation"));

            // Act
            var result = _controller.Rotate(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult.Value);
        }

        [TestMethod]
        public void Rotate_GeneralException_ReturnsStatusCode500()
        {
            // Arrange
            var request = new RotationRequest
            {
                Side = RubikCubeSideEnum.Front,
                Clockwise = true
            };
            _processorMock.Setup(p => p.Rotate(request))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = _controller.Rotate(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [TestMethod]
        public void Reset_ValidRequest_ReturnsJsonWithMatrix()
        {
            // Arrange
            var expectedMatrix = new int[][] { new int[] { 1, 2, 3 } };
            _processorMock.Setup(p => p.Reset()).Returns(expectedMatrix);

            // Act
            var result = _controller.Reset();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
            _processorMock.Verify(p => p.Reset(), Times.Once);
        }

        [TestMethod]
        public void Reset_Exception_ReturnsStatusCode500()
        {
            // Arrange
            _processorMock.Setup(p => p.Reset())
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = _controller.Reset();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
    }
}