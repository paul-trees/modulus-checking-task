using System;
using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Strategies;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Strategies
{
    public class DoubleAlternateModulusCheckStrategyTests
    {
        #region Fields

        private readonly DoubleAlternateModulusCheckStrategy _sut;

        #endregion

        #region Constructor

        public DoubleAlternateModulusCheckStrategyTests()
        {
            _sut = new DoubleAlternateModulusCheckStrategy();
        }

        #endregion

        #region Tests

        [Fact]
        public void IsApplicable_ReturnsTrueWhenMethodNameIsDBLAL()
        {
            // Act
            var result = _sut.IsApplicable("DBLAL");

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("MOD10")]
        [InlineData("MOD11")]
        public void IsApplicable_ReturnsFalseWhenMethodNameIsNotDBLAL(string methodName)
        {
            // Act
            var result = _sut.IsApplicable(methodName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ThrowsArgumentExceptionAsPassedModulusWeightEntityIsForADifferentStrategy()
        {
            // Arrange
            var modulusWeight = new ModulusWeightEntity {ModCheck = "Fred"};

            // Act
            Action act = () => _sut.IsValid(new List<int>(), "12345678", modulusWeight);

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage($"Check applicable to provided { nameof(ModulusWeightEntity)} does not match DBLAL.");
        }       

        [Fact]
        public void IsValid_ReturnsTrue()
        {
            // Arrange - Sort Code = 202959 & Account Number = 63748472
            var modulusWeightsList = new List<int> { 4, 0, 4, 9, 10, 9, 12, 3, 14, 4, 16, 4, 14, 2 };
            var modulusWeight = CreateTestModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "63748472", modulusWeight);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_ReturnsFalse()
        {
            // Arrange - Sort Code = 203099 & Account Number = 66831036
            var modulusWeightsList = new List<int> { 4, 0, 6, 0, 18, 9, 12, 6, 16, 3, 2, 0, 6, 6 };
            var modulusWeight = CreateTestModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "63748472", modulusWeight);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Test Helpers

        private static ModulusWeightEntity CreateTestModulusWeightEntity()
        {
            return new ModulusWeightEntity
            {
                SortCodeRangeStart = 202700,
                SortCodeRangeEnd = 203239,
                ModCheck = "DBLAL",
                WeightU = 2,
                WeightV = 1,
                WeightW = 2,
                WeightX = 1,
                WeightY = 2,
                WeightZ = 1,
                WeightA = 2,
                WeightB = 1,
                WeightC = 2,
                WeightD = 1,
                WeightE = 2,
                WeightF = 1,
                WeightG = 2,
                WeightH = 1,
                ExceptionCode = "6"
            };
        }

        #endregion
    }
}
