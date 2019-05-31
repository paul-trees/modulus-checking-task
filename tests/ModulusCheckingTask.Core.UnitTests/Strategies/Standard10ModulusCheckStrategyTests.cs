using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Strategies;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Strategies
{
    public class Standard10ModulusCheckStrategyTests
    {
        #region Fields

        private readonly Standard10ModulusCheckStrategy _sut;

        #endregion

        #region Constructor

        public Standard10ModulusCheckStrategyTests()
        {
            _sut = new Standard10ModulusCheckStrategy();
        }

        #endregion

        #region Tests

        [Fact]
        public void IsApplicable_ReturnsTrueWhenMethodNameIsMOD10()
        {
            // Act
            var result = _sut.IsApplicable("MOD10");

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("DBLAL")]
        [InlineData("MOD11")]
        public void IsApplicable_ReturnsFalseWhenMethodNameIsNotMOD10(string methodName)
        {
            // Act
            var result = _sut.IsApplicable(methodName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ReturnsTrue()
        {
            // Arrange
            var modulusWeightsList = new List<int> { 0, 0, 0, 0, 0, 0, 42, 6, 9, 49, 4, 27, 35, 8 };
            var modulusWeight = CreateTestModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "66374958", modulusWeight);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_ReturnsFalse()
        {
            // Arrange
            var modulusWeightsList = new List<int> { 0, 0, 0, 0, 0, 0, 42, 6, 9, 49, 4, 27, 35, 9 };
            var modulusWeight = CreateTestModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "66374959", modulusWeight);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Test Helpers

        private static ModulusWeightEntity CreateTestModulusWeightEntity()
        {            
            return new ModulusWeightEntity
            {
                SortCodeRangeStart = 089000,
                SortCodeRangeEnd = 089999,
                ModCheck = "MOD10",
                WeightU = 0,
                WeightV = 0,
                WeightW = 0,
                WeightX = 0,
                WeightY = 0,
                WeightZ = 0,
                WeightA = 7,
                WeightB = 1,
                WeightC = 3,
                WeightD = 7,
                WeightE = 1,
                WeightF = 3,
                WeightG = 7,
                WeightH = 1,
                ExceptionCode = string.Empty
            };
        }

        #endregion
    }
}
