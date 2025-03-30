using TreasureHunt.Core.Models;

namespace TreasureHunt.Core.Tests.Models
{
    public class TreasureMapRequestTests
    {
        [Test]
        public void IsValid_ShouldReturnTrue_ForValidRequest()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 5 },
                    { "1,2", 1 },
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.True(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForInvalidN()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 0, // Invalid
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 5 },
                    { "1,2", 1 },
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForInvalidM()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 0, // Invalid
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 5 },
                    { "1,2", 1 },
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForInvalidP()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 10, // Invalid
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 5 },
                    { "1,2", 1 },
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForInvalidMatrixDimensions()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 5 }
                    // Missing elements
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForInvalidMatrixValues()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 6 }, // Invalid value
                    { "1,2", 1 },
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForMultipleChestsNumberedP()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 5 },
                    { "1,2", 5 }, // Multiple chests numbered P
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForNoChestNumberedP()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 1 },
                    { "0,1", 2 },
                    { "0,2", 3 },
                    { "1,0", 4 },
                    { "1,1", 4 }, // No chest numbered P
                    { "1,2", 1 },
                    { "2,0", 2 },
                    { "2,1", 3 },
                    { "2,2", 4 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        // New test cases for 100% coverage

        [Test]
        public void IsValid_ShouldReturnFalse_ForNullMatrix()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 5,
                Matrix = null
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForMaximumBoundaryValues()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 501, // Exceeds maximum
                M = 3,
                P = 5,
                Matrix = new Dictionary<string, int>()
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void ToDimensionalArray_ShouldConvertDictionaryToArray()
        {
            // Arrange
            var dict = new Dictionary<string, int>
            {
                { "0,0", 1 },
                { "0,1", 2 },
                { "1,0", 3 },
                { "1,1", 4 }
            };

            // Act
            var result = dict.ToDimensionalArray(2, 2);

            // Assert
            Assert.That(result[0, 0], Is.EqualTo(1));
            Assert.That(result[0, 1], Is.EqualTo(2));
            Assert.That(result[1, 0], Is.EqualTo(3));
            Assert.That(result[1, 1], Is.EqualTo(4));
        }

        [Test]
        public void ToDimensionalArray_ShouldHandleInvalidCoordinates()
        {
            // Arrange
            var dict = new Dictionary<string, int>
            {
                { "invalid", 1 },        // Invalid format
                { "0,0", 2 },            // Valid
                { "-1,0", 3 },           // Out of range
                { "5,5", 4 },            // Out of range
                { "0,", 5 },             // Invalid format
                { "0,0,0", 6 }           // Invalid format
            };

            // Act
            var result = dict.ToDimensionalArray(2, 2);

            // Assert
            Assert.That(result[0, 0], Is.EqualTo(2)); // Only valid coordinate should be set
            Assert.That(result[0, 1], Is.EqualTo(0)); // Default value for invalid coordinates
            Assert.That(result[1, 0], Is.EqualTo(0));
            Assert.That(result[1, 1], Is.EqualTo(0));
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForPLessThanOne()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 3,
                M = 3,
                P = 0, // Less than 1
                Matrix = new Dictionary<string, int>()
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForMatrixValueLessThanOne()
        {
            // Arrange
            var request = new TreasureMapRequest
            {
                N = 2,
                M = 2,
                P = 2,
                Matrix = new Dictionary<string, int>
                {
                    { "0,0", 0 }, // Less than 1
                    { "0,1", 1 },
                    { "1,0", 1 },
                    { "1,1", 2 }
                }
            };

            // Act
            var result = request.IsValid();

            // Assert
            Assert.False(result);
        }
    }
}