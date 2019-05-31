using System;
using FluentAssertions;
using ModulusCheckingTask.Infrastructure.Mappers;
using Xunit;

namespace ModulusCheckingTasp.Infrastructure.UnitTests.Mappers
{
    public class ModulusWeightEntityMapperTests
    {
        #region Fields

        private readonly ModulusWeightEntityMapper _sut;

        #endregion

        #region Constructor

        public ModulusWeightEntityMapperTests()
        {
            _sut = new ModulusWeightEntityMapper();
        }

        #endregion

        #region Tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_ThrowsArgumentExceptionWhenInputStringIsNullOrEmpty(string inputLine)
        {
            // Act
            Action act = () => _sut.Create(inputLine);

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*modulusWeightData*");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16")]
        [InlineData("1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19")]
        public void Create_ThrowsArgumentOutOfRangeExceptionWhenInputStringDoesNotContain17Or18Elements(string inputLine)
        {
            // Act
            Action act = () => _sut.Create(inputLine);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().WithMessage("*modulusWeightData*");
        }

        [Fact]
        public void Create_SuccessfullyCreatesEntityWithoutExceptionNumber()
        {
            // Arrange
            var dataLine = "123 456 ABC 1 2 3 4 5 6 10 11 12 13 14 15 16 17";

            // Act
            var result = _sut.Create(dataLine);

            // Assert
            result.SortCodeRangeStart.Should().Be(123);
            result.SortCodeRangeEnd.Should().Be(456);
            result.ModCheck.Should().Be("ABC");
            result.WeightU.Should().Be(1);
            result.WeightV.Should().Be(2);
            result.WeightW.Should().Be(3);
            result.WeightX.Should().Be(4);
            result.WeightY.Should().Be(5);
            result.WeightZ.Should().Be(6);
            result.WeightA.Should().Be(10);
            result.WeightB.Should().Be(11);
            result.WeightC.Should().Be(12);
            result.WeightD.Should().Be(13);
            result.WeightE.Should().Be(14);
            result.WeightF.Should().Be(15);
            result.WeightG.Should().Be(16);
            result.WeightH.Should().Be(17);
            result.ExceptionCode.Should().BeNull();
        }

        [Fact]
        public void Create_SuccessfullyCreatesEntityWithExceptionNumber()
        {
            // Arrange
            var dataLine = "123 456 ABC 1 2 3 4 5 6 10 11 12 13 14 15 16 17 E1";

            // Act
            var result = _sut.Create(dataLine);

            // Assert
            result.SortCodeRangeStart.Should().Be(123);
            result.SortCodeRangeEnd.Should().Be(456);
            result.ModCheck.Should().Be("ABC");
            result.WeightU.Should().Be(1);
            result.WeightV.Should().Be(2);
            result.WeightW.Should().Be(3);
            result.WeightX.Should().Be(4);
            result.WeightY.Should().Be(5);
            result.WeightZ.Should().Be(6);
            result.WeightA.Should().Be(10);
            result.WeightB.Should().Be(11);
            result.WeightC.Should().Be(12);
            result.WeightD.Should().Be(13);
            result.WeightE.Should().Be(14);
            result.WeightF.Should().Be(15);
            result.WeightG.Should().Be(16);
            result.WeightH.Should().Be(17);
            result.ExceptionCode.Should().Be("E1");
        }

        #endregion
    }
}
