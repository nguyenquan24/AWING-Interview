using Microsoft.AspNetCore.Mvc;
using Moq;
using TreasureHunt.API.Controllers;
using TreasureHunt.Core.Entities;
using TreasureHunt.Core.Interfaces;
using TreasureHunt.Core.Models;

namespace TreasureHunt.API.Tests.Controllers
{
    [TestFixture]
    public class TreasureHuntControllerTests
    {
        private Mock<ITreasureHuntService> _mockService;
        private TreasureHuntController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITreasureHuntService>();
            _controller = new TreasureHuntController(_mockService.Object);
        }

        [Test]
        public async Task SolveTreasureHunt_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 2,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 1 },
                    { "1,0", 2 }, { "1,1", 1 }, { "1,2", 2 },
                    { "2,0", 1 }, { "2,1", 2 }, { "2,2", 1 }
                }
            };
            var expectedResult = 15.5;
            _mockService.Setup(s => s.SolveTreasureHuntAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SolveTreasureHunt(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task SolveTreasureHunt_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");
            var request = new TreasureMapRequest();

            // Act
            var result = await _controller.SolveTreasureHunt(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task SolveTreasureHunt_ServiceThrowsException_ThrowsException()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 2,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 1 },
                    { "1,0", 2 }, { "1,1", 1 }, { "1,2", 2 },
                    { "2,0", 1 }, { "2,1", 2 }, { "2,2", 1 }
                }
            };
            _mockService.Setup(s => s.SolveTreasureHuntAsync(request))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _controller.SolveTreasureHunt(request));
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
        }

        [Test]
        public async Task GetHistory_ReturnsOkResult()
        {
            // Arrange
            var expectedHistory = new List<TreasureMap>
            {
                new TreasureMap
                {
                    Id = 1,
                    N = 3,
                    M = 3,
                    P = 2,
                    MatrixData = "{\"0,0\":1,\"0,1\":2,\"0,2\":1}",
                    Result = 15.5,
                    CreatedAt = DateTime.UtcNow
                }
            };
            _mockService.Setup(s => s.GetHistoryAsync())
                .ReturnsAsync(expectedHistory);

            // Act
            var result = await _controller.GetHistory();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var actualHistory = okResult.Value as IEnumerable<TreasureMap>;
            Assert.That(actualHistory, Is.Not.Null);
            Assert.That(actualHistory.Count(), Is.EqualTo(expectedHistory.Count));
            Assert.That(actualHistory.First().Id, Is.EqualTo(expectedHistory.First().Id));
            Assert.That(actualHistory.First().Result, Is.EqualTo(expectedHistory.First().Result));
        }

        [Test]
        public async Task GetHistory_EmptyList_ReturnsOkResultWithEmptyList()
        {
            // Arrange
            var expectedHistory = new List<TreasureMap>();
            _mockService.Setup(s => s.GetHistoryAsync())
                .ReturnsAsync(expectedHistory);

            // Act
            var result = await _controller.GetHistory();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var actualHistory = okResult.Value as IEnumerable<TreasureMap>;
            Assert.That(actualHistory, Is.Empty);
            _mockService.Verify(s => s.GetHistoryAsync(), Times.Once);
        }

        [Test]
        public async Task GetHistory_ServiceThrowsException_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetHistoryAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _controller.GetHistory());
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
        }

        [Test]
        public async Task SolveTreasureHunt_ServiceReturnsZero_ReturnsOkResult()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 2,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 1 },
                    { "1,0", 2 }, { "1,1", 1 }, { "1,2", 2 },
                    { "2,0", 1 }, { "2,1", 2 }, { "2,2", 1 }
                }
            };
            _mockService.Setup(s => s.SolveTreasureHuntAsync(request))
                .ReturnsAsync(0.0);

            // Act
            var result = await _controller.SolveTreasureHunt(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(0.0));
        }
    }
}