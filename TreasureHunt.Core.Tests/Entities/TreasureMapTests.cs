using System.Text.Json;
using TreasureHunt.Core.Entities;

namespace TreasureHunt.Core.Tests.Entities
{
    public class TreasureMapTests
    {
        [Test]
        public void TreasureMap_ShouldSetAndGetProperties_Successfully()
        {
            // Arrange
            var expectedId = 1;
            var expectedN = 3;
            var expectedM = 4;
            var expectedP = 5;
            var matrix = new List<List<int>>
            {
                new List<int> { 1, 2, 3, 4 },
                new List<int> { 2, 3, 4, 5 },
                new List<int> { 3, 4, 5, 1 }
            };
            var expectedMatrixData = JsonSerializer.Serialize(matrix);
            var expectedResult = 12.5;
            var expectedCreatedAt = DateTime.UtcNow;

            // Act
            var treasureMap = new TreasureMap
            {
                Id = expectedId,
                N = expectedN,
                M = expectedM,
                P = expectedP,
                MatrixData = expectedMatrixData,
                Result = expectedResult,
                CreatedAt = expectedCreatedAt
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treasureMap.Id, Is.EqualTo(expectedId));
                Assert.That(treasureMap.N, Is.EqualTo(expectedN));
                Assert.That(treasureMap.M, Is.EqualTo(expectedM));
                Assert.That(treasureMap.P, Is.EqualTo(expectedP));
                Assert.That(treasureMap.MatrixData, Is.EqualTo(expectedMatrixData));
                Assert.That(treasureMap.Result, Is.EqualTo(expectedResult));
                Assert.That(treasureMap.CreatedAt, Is.EqualTo(expectedCreatedAt));
            });
        }

        [Test]
        public void TreasureMap_ShouldHandleDefaultValues()
        {
            // Arrange & Act
            var treasureMap = new TreasureMap();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treasureMap.Id, Is.EqualTo(0));
                Assert.That(treasureMap.N, Is.EqualTo(0));
                Assert.That(treasureMap.M, Is.EqualTo(0));
                Assert.That(treasureMap.P, Is.EqualTo(0));
                Assert.That(treasureMap.MatrixData, Is.Null);
                Assert.That(treasureMap.Result, Is.EqualTo(0));
                Assert.That(treasureMap.CreatedAt, Is.EqualTo(default(DateTime)));
            });
        }

        [Test]
        public void TreasureMap_ShouldHandleLargeMatrix()
        {
            // Arrange
            var n = 100;
            var m = 100;
            var matrix = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                var row = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    row.Add((i * m + j) % 5 + 1); // Values from 1 to 5
                }
                matrix.Add(row);
            }
            var matrixData = JsonSerializer.Serialize(matrix);

            // Act
            var treasureMap = new TreasureMap
            {
                N = n,
                M = m,
                P = 5,
                MatrixData = matrixData,
                Result = 250.5
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treasureMap.N, Is.EqualTo(n));
                Assert.That(treasureMap.M, Is.EqualTo(m));
                Assert.That(treasureMap.MatrixData, Is.EqualTo(matrixData));
                Assert.That(treasureMap.MatrixData.Length, Is.GreaterThan(0));
            });
        }

        [Test]
        public void TreasureMap_ShouldHandleSpecialCharactersInMatrix()
        {
            // Arrange
            var matrix = new List<List<int>>
            {
                new List<int> { 1, 2 },
                new List<int> { 3, 4 }
            };
            var matrixData = JsonSerializer.Serialize(matrix)
                .Replace("[", "《")
                .Replace("]", "》");

            // Act
            var treasureMap = new TreasureMap
            {
                N = 2,
                M = 2,
                P = 4,
                MatrixData = matrixData
            };

            // Assert
            Assert.That(treasureMap.MatrixData, Is.EqualTo(matrixData));
        }

        [Test]
        public void TreasureMap_ShouldHandleNegativeResult()
        {
            // Arrange & Act
            var treasureMap = new TreasureMap
            {
                N = 2,
                M = 2,
                P = 4,
                Result = -123.45
            };

            // Assert
            Assert.That(treasureMap.Result, Is.EqualTo(-123.45));
        }

        [Test]
        public void TreasureMap_ShouldHandleEmptyMatrix()
        {
            // Arrange & Act
            var treasureMap = new TreasureMap
            {
                N = 0,
                M = 0,
                P = 0,
                MatrixData = "[]"
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treasureMap.N, Is.EqualTo(0));
                Assert.That(treasureMap.M, Is.EqualTo(0));
                Assert.That(treasureMap.MatrixData, Is.EqualTo("[]"));
            });
        }

        [Test]
        public void TreasureMap_ShouldHandleMaxValues()
        {
            // Arrange & Act
            var treasureMap = new TreasureMap
            {
                Id = int.MaxValue,
                N = 500,
                M = 500,
                P = 250000,
                Result = double.MaxValue
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treasureMap.Id, Is.EqualTo(int.MaxValue));
                Assert.That(treasureMap.N, Is.EqualTo(500));
                Assert.That(treasureMap.M, Is.EqualTo(500));
                Assert.That(treasureMap.P, Is.EqualTo(250000));
                Assert.That(treasureMap.Result, Is.EqualTo(double.MaxValue));
            });
        }

        [Test]
        public void TreasureMap_ShouldHandleInvalidMatrixJson()
        {
            // Arrange & Act
            var treasureMap = new TreasureMap
            {
                N = 2,
                M = 2,
                P = 4,
                MatrixData = "invalid json"
            };

            // Assert
            Assert.That(treasureMap.MatrixData, Is.EqualTo("invalid json"));
        }

        [Test]
        public void TreasureMap_ShouldHandleMinValues()
        {
            // Arrange & Act
            var treasureMap = new TreasureMap
            {
                Id = int.MinValue,
                N = 1,
                M = 1,
                P = 1,
                Result = double.MinValue
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treasureMap.Id, Is.EqualTo(int.MinValue));
                Assert.That(treasureMap.N, Is.EqualTo(1));
                Assert.That(treasureMap.M, Is.EqualTo(1));
                Assert.That(treasureMap.P, Is.EqualTo(1));
                Assert.That(treasureMap.Result, Is.EqualTo(double.MinValue));
            });
        }
    }
} 