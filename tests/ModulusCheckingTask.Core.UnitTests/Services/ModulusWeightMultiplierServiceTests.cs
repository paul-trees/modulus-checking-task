using System;
using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Services;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Services
{
    public class ModulusWeightMultiplierServiceTests
    {
        #region Fields

        private readonly ModulusWeightMultiplierService _sut;

        #endregion

        #region Constants

        private const string CombinedSortCodeAndAccountNumber = "23456789123456";

        #endregion

        #region Constructor

        public ModulusWeightMultiplierServiceTests()
        {
            _sut = new ModulusWeightMultiplierService();
        }

        #endregion

        #region Tests

        [Fact]
        public void Execute_ThrowsArgumentNullExceptionsAsNullModulusWeightsListProvided()
        {
            // Act
            Action act = () => _sut.Execute(CombinedSortCodeAndAccountNumber, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*modulusWeights*");
        }

        [Theory]
        [InlineData("1234567890123")]
        [InlineData("123456789012345")]
        [InlineData("1234567890123A")]
        [InlineData("")]
        public void Execute_ThrowsArgumentExceptionAsInvalidCombinedSortCodeAndAccountNumberProvided(string combinedSortCodeAndAccountNumber)
        {
            // Act
            Action act = () => _sut.Execute(combinedSortCodeAndAccountNumber, new List<int>());

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*combinedSortCodeAndAccountNumber*");
        }

        [Fact]
        public void Execute_ThrowsArgumentNullExceptionWhenNullCombinedSortCodeAndAccountNumberProvided()
        {
            // Act
            Action act = () => _sut.Execute(null, new List<int>());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*input*");
        }

        [Fact]
        public void Execute_ThrowsArgumentExceptionAsCombinedSortCodeAndAccountNumberDigitsDoNotMatchCountOfModulusWeights()
        {
            // Act
            Action act = () => _sut.Execute(CombinedSortCodeAndAccountNumber, new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage($"Expected number of digits in combined sort code and account number (14) to match the count of modulus weights (13).");
        }

        [Fact]
        public void Execute_CorrectlyMultipliesTheSortCodeAndAccountNumberByTheCorrectModulusWeightsAndReturnsResult()
        {
            // Act
            var result = _sut.Execute(CombinedSortCodeAndAccountNumber, new List<int>{ 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

            // Assert
            result.Should().ContainInOrder(new[] { 4, 9, 16, 25, 36, 49, 64, 81, 10, 22, 36, 52, 70, 90 });
        }

        #endregion
    }
}
