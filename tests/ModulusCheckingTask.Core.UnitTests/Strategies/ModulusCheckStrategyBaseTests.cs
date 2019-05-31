using System;
using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Strategies;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Strategies
{
    public class ModulusCheckStrategyBaseTests
    {
        #region Fields

        private readonly DoubleAlternateModulusCheckStrategy _sut;

        #endregion

        #region Constants

        private const string ValidAccountNumber = "12345678";

        #endregion

        #region Constructor

        public ModulusCheckStrategyBaseTests()
        {
            _sut = new DoubleAlternateModulusCheckStrategy();
        }

        #endregion

        #region Tests

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionForNullResultsList()
        {
            // Act
            Action act = () => _sut.IsValid(null, ValidAccountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*resultsList*");
        }

        [Theory]
        [InlineData("ABC12345")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("")]
        public void IsValid_ThrowsArgumentExceptionAsAccountNumberIsNot6Digits(string accountNumber)
        {
            // Act
            Action act = () => _sut.IsValid(new List<int>(), accountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*accountNumber*");
        }

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionForNullAccountNumber()
        {
            // Act
            Action act = () => _sut.IsValid(new List<int>(), null, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*input*");
        }

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionForNullModulusWeightEntity()
        {
            // Act
            Action act = () => _sut.IsValid(new List<int>(), ValidAccountNumber, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*modulusWeight*");
        }

        #endregion
    }
}
