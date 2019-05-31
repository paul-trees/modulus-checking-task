using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Strategies;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Strategies
{
    public class Standard11ModulusCheckStrategyTests
    {
        #region Fields

        private readonly Standard11ModulusCheckStrategy _sut;

        #endregion

        #region Constructor

        public Standard11ModulusCheckStrategyTests()
        {
            _sut = new Standard11ModulusCheckStrategy();
        }

        #endregion

        #region Tests

        [Fact]
        public void IsApplicable_ReturnsTrueWhenMethodNameIsMOD11()
        {
            // Act
            var result = _sut.IsApplicable("MOD11");

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("MOD10")]
        [InlineData("DBLAL")]
        public void IsApplicable_ReturnsFalseWhenMethodNameIsNotMOD11(string methodName)
        {
            // Act
            var result = _sut.IsApplicable(methodName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ReturnsTrue()
        {
            // Arrange - Sort Code = 203099 & Account Number = 66831036
            var modulusWeightsList = new List<int> { 4, 0, 6, 0, 18, 9, 12, 6, 16, 3, 2, 0, 6, 6 };
            var modulusWeight = CreateTestModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "66831036", modulusWeight);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_ReturnsFalse()
        {
            // Arrange - Sort Code = 203099 & Account Number = 58716970
            var modulusWeightsList = new List<int> { 4, 0, 6, 0, 18, 9, 10, 8, 14, 1, 12, 9, 14, 0 };
            var modulusWeight = CreateTestModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "58716970", modulusWeight);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ReturnsTrueForException7()
        {
            // Arrange - Sort Code = 772798 & Account Number = 99345694
            var modulusWeightsList = new List<int> { 0, 0, 2, 14, 45, 24, 54, 36, 24, 28, 50, 54, 27, 4 };
            var modulusWeight = CreateTestException7ModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList, "99345694", modulusWeight);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_ReturnsTrueForException4()
        {
            // Arrange - Sort Code = 134020 & Account Number = 63849203
            var modulusWeightsList = new List<int> { 0, 0, 0, 0, 10, 0, 48, 12, 48, 12, 45, 4, 0, 0 };
            var modulusWeight = CreateTestException4ModulusWeightEntity();

            // Act
            var result = _sut.IsValid(modulusWeightsList,  "63849203", modulusWeight);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Test Helpers

        private static ModulusWeightEntity CreateTestModulusWeightEntity()
        {
            return new ModulusWeightEntity
            {
                SortCodeRangeStart = 202700,
                SortCodeRangeEnd = 203239,
                ModCheck = "MOD11",
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

        private static ModulusWeightEntity CreateTestException7ModulusWeightEntity()
        {
            return new ModulusWeightEntity
            {
                SortCodeRangeStart = 771900,
                SortCodeRangeEnd = 772799,
                ModCheck = "MOD11",
                WeightU = 0,
                WeightV = 0,
                WeightW = 1,
                WeightX = 2,
                WeightY = 5,
                WeightZ = 3,
                WeightA = 6,
                WeightB = 4,
                WeightC = 8,
                WeightD = 7,
                WeightE = 10,
                WeightF = 9,
                WeightG = 3,
                WeightH = 1,
                ExceptionCode = "7"
            };
        }

        private static ModulusWeightEntity CreateTestException4ModulusWeightEntity()
        {
            return new ModulusWeightEntity
            {
                SortCodeRangeStart = 134012,
                SortCodeRangeEnd = 134020,
                ModCheck = "MOD11",
                WeightU = 0,
                WeightV = 0,
                WeightW = 0,
                WeightX = 7,
                WeightY = 5,
                WeightZ = 9,
                WeightA = 8,
                WeightB = 4,
                WeightC = 6,
                WeightD = 3,
                WeightE = 5,
                WeightF = 2,
                WeightG = 0,
                WeightH = 0,
                ExceptionCode = "4"
            };
        }

        #endregion
    }
}
