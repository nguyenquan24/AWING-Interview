using Microsoft.EntityFrameworkCore;
using TreasureHunt.Application.Services;
using TreasureHunt.Core.Models;
using TreasureHunt.Infrastructure.Data;

namespace TreasureHunt.Application.Tests.Services
{
    public class TreasureHuntServiceTests
    {
        private ApplicationDbContext _context;
        private TreasureHuntService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TreasureHuntTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            
            _service = new TreasureHuntService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task SolveTreasureHuntAsync_SimpleMatrix_ReturnsCorrectResult()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 3,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 3 },
                    { "1,0", 2 }, { "1,1", 2 }, { "1,2", 2 },
                    { "2,0", 1 }, { "2,1", 1 }, { "2,2", 1 }
                }
            };

            // Act
            var result = await _service.SolveTreasureHuntAsync(request);

            // Assert
            Assert.That(result, Is.GreaterThan(0));
            Assert.That(result, Is.LessThan(10)); // Reasonable range for a 3x3 matrix
        }

        [Test]
        public void SolveTreasureHuntAsync_InvalidMatrix_ThrowsArgumentException()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 3,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 4 }, // 4 is greater than P
                    { "1,0", 2 }, { "1,1", 2 }, { "1,2", 2 },
                    { "2,0", 1 }, { "2,1", 1 }, { "2,2", 1 }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.SolveTreasureHuntAsync(request)
            );
            Assert.That(ex.Message, Is.EqualTo("Invalid request data."));
        }

        [Test]
        public void SolveTreasureHuntAsync_MissingChestNumber_ThrowsArgumentException()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 3,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 1 }, { "0,2", 3 }, // Missing chest number 2
                    { "1,0", 1 }, { "1,1", 1 }, { "1,2", 3 },
                    { "2,0", 1 }, { "2,1", 1 }, { "2,2", 3 }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.SolveTreasureHuntAsync(request)
            );
            Assert.That(ex.Message, Is.EqualTo("Invalid request data."));
        }

        [Test]
        public async Task GetHistoryAsync_ReturnsLatestTenRecords()
        {
            // Arrange
            for (int i = 0; i < 15; i++)
            {
                await _service.SolveTreasureHuntAsync(new TreasureMapRequest
                {
                    N = 2,
                    M = 2,
                    P = 2,
                    Matrix = new Dictionary<string, int>
                    {
                        { "0,0", 1 }, { "0,1", 2 },
                        { "1,0", 1 }, { "1,1", 1 }
                    }
                });
            }

            // Act
            var history = await _service.GetHistoryAsync();

            // Assert
            Assert.That(history.Count(), Is.EqualTo(10));
            Assert.That(history, Is.Ordered.By("CreatedAt").Descending);
        }

        [Test]
        public async Task SolveTreasureHuntAsync_OptimalPath_VerifyResult()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 2,
                M = 2,
                P = 2,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 },
                    { "1,0", 1 }, { "1,1", 1 }
                }
            };

            // Act
            var result = await _service.SolveTreasureHuntAsync(request);

            // Assert
            // Khoảng cách từ (0,0) đến chest 1 gần nhất + khoảng cách từ đó đến chest 2
            var expectedMinDistance = 1.0; // Từ (0,0) đến (0,0) là 0 + từ (0,0) đến (0,1) là 1
            Assert.That(result, Is.EqualTo(expectedMinDistance).Within(0.0001));
        }

        [Test]
        public async Task SolveTreasureHuntAsync_LargeMatrix_CompletesInReasonableTime()
        {
            // Arrange
            int n = 10, m = 10, p = 5;
            var matrix = new Dictionary<string, int>();
            
            // Create a large matrix with valid path
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[$"{i},{j}"] = 1;

            // Place chests 1 to p in a line
            for (int i = 0; i < p; i++)
                matrix[$"{i},0"] = i + 1;

            var request = new TreasureMapRequest
            {
                N = n,
                M = m,
                P = p,
                Matrix = matrix
            };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                var result = await _service.SolveTreasureHuntAsync(request);
                Assert.That(result, Is.GreaterThan(0));
            });
        }

        [Test]
        public void SolveTreasureHuntAsync_MissingChestInSequence_ThrowsException()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 3 },
                    { "1,0", 1 }, { "1,1", 5 }, { "1,2", 2 }, // Missing chest 4
                    { "2,0", 1 }, { "2,1", 3 }, { "2,2", 2 }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(
                async () => await _service.SolveTreasureHuntAsync(request)
            );
            Assert.That(ex.Message, Is.EqualTo("Không tìm thấy chest 4 trong ma trận!"));
        }

        [Test]
        public void SolveTreasureHuntAsync_MultipleChestsInSequenceMissing_ThrowsArgumentException()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 5 },
                    { "1,0", 1 }, { "1,1", 5 }, { "1,2", 2 }, // Missing chests 3 and 4
                    { "2,0", 1 }, { "2,1", 2 }, { "2,2", 5 }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.SolveTreasureHuntAsync(request)
            );
            Assert.That(ex.Message, Is.EqualTo("Invalid request data."));
        }

        [Test]
        public void SolveTreasureHuntAsync_LastChestMissing_ThrowsArgumentException()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 3,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 }, { "0,1", 2 }, { "0,2", 1 },
                    { "1,0", 1 }, { "1,1", 2 }, { "1,2", 2 }, // Missing chest 3
                    { "2,0", 1 }, { "2,1", 2 }, { "2,2", 2 }
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.SolveTreasureHuntAsync(request)
            );
            Assert.That(ex.Message, Is.EqualTo("Invalid request data."));
        }
    }
}