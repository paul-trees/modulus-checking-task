using System;
using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Adapters;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Services;
using ModulusCheckingTask.Core.Strategies;
using NSubstitute;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Services
{
    public class ModulusCheckingServiceTests
    {
        #region Fields

        private readonly IModulusWeightEntityAdapter _modulusWeightEntityAdapter;

        #endregion

        #region Constants

        private const string ValidSortCode = "123456";
        private const string ValidAccountNumber = "12345678";
        private const string ValidModulusMethodName = "Mod10";

        #endregion

        #region Constructor

        public ModulusCheckingServiceTests()
        {
            _modulusWeightEntityAdapter = Substitute.For<IModulusWeightEntityAdapter>();
        }

        #endregion

        #region Tests

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionBecauseNullSortCodePassed()
        {
            // Assert
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>(), _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(null, ValidAccountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*input*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("12345")]
        [InlineData("1234567")]
        [InlineData("12345A")]
        public void IsValid_ThrowsArgumentExceptionBecauseInvalidSortCodePassed(string invalidSortCode)
        {
            // Assert
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>(), _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(invalidSortCode, ValidAccountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*sortCode*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("1234567A")]
        public void IsValid_ThrowsArgumentExceptionBecauseInvalidAccountNumberPassed(string invalidAccountNumber)
        {
            // Assert
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>(), _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(ValidSortCode, invalidAccountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*accountNumber*");
        }

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionBecauseNullAccountNumberPassed()
        {
            // Assert
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>(), _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(ValidSortCode, null, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*input*");
        }

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionBecauseNullModulusWeightEntityPassed()
        {
            // Arrange
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>(), _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(ValidSortCode, ValidAccountNumber, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*modulusWeight*");
        }

        [Fact]
        public void IsValid_ThrowsInvalidOperationExceptionAsNoApplicableStrategyFound()
        {
            // Arrange
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>
            {
                CreateModulusCheckStrategy(true, new ModulusWeightEntity { ModCheck = "NotValid" })
            }, _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(ValidSortCode, ValidAccountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Sequence contains no matching element");
        }

        [Fact]
        public void IsValid_ThrowsInvalidOperationExceptionAsMoreThanOneApplicableStrategyFound()
        {
            // Arrange
            var modulusWeight = new ModulusWeightEntity{ModCheck = ValidModulusMethodName};
            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>
            {
                CreateModulusCheckStrategy(true, modulusWeight),
                CreateModulusCheckStrategy(true, modulusWeight)
            }, _modulusWeightEntityAdapter);

            // Act
            Action act = () => sut.IsValid(ValidSortCode, ValidAccountNumber, modulusWeight);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Sequence contains more than one matching element");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsValid_ExecutesSuccessfully(bool isValidResult)
        {
            // Arrange
            var modulusWeight = new ModulusWeightEntity { ModCheck = ValidModulusMethodName };
            var modulusWeightsList = new List<int> { 1 };
            _modulusWeightEntityAdapter.Execute(ValidSortCode + ValidAccountNumber, modulusWeight).Returns(modulusWeightsList);

            var sut = new ModulusCheckingService(new List<IModulusCheckStrategy>
            {
                CreateModulusCheckStrategy(isValidResult, modulusWeight),
            }, _modulusWeightEntityAdapter);

            // Act
            var result = sut.IsValid(ValidSortCode, ValidAccountNumber, modulusWeight);

            // Assert
            result.Should().Be(isValidResult);
            _modulusWeightEntityAdapter.Received(1).Execute(ValidSortCode + ValidAccountNumber, modulusWeight);
        }

        #endregion

        #region Test Helpers

        private IModulusCheckStrategy CreateModulusCheckStrategy(bool isValidResult, ModulusWeightEntity checkEntity, string accountNumber = ValidAccountNumber)
        {
            var checkStrategy = Substitute.For<IModulusCheckStrategy>();
            checkStrategy.IsApplicable(checkEntity.ModCheck).Returns(true);
            checkStrategy.IsValid(Arg.Any<IEnumerable<int>>(), accountNumber, checkEntity).Returns(isValidResult);
            return checkStrategy;
        }

        #endregion
    }
}
