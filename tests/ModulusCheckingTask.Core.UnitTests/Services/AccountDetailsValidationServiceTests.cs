using System;
using System.Collections.Generic;
using FluentAssertions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Exceptions;
using ModulusCheckingTask.Core.Repositories;
using ModulusCheckingTask.Core.Services;
using NSubstitute;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Services
{
    public class AccountDetailsValidationServiceTests
    {
        #region Fields

        private readonly IModulusWeightRepository _modulusWeightRepository;
        private readonly IModulusCheckingService _modulusCheckingService;
        private readonly AccountDetailsValidationService _sut;

        #endregion

        #region Constants

        private const int ValidSortCodeAsInt = 123456;
        private const string ValidSortCode = "123456";
        private const string ValidAccountNumber = "12345678";

        #endregion

        #region Constructor

        public AccountDetailsValidationServiceTests()
        {
            _modulusWeightRepository = Substitute.For<IModulusWeightRepository>();
            _modulusCheckingService = Substitute.For<IModulusCheckingService>();

            _sut = new AccountDetailsValidationService(_modulusWeightRepository, _modulusCheckingService);
        }

        #endregion

        #region Tests

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionBecauseNullSortCodePassed()
        {
            // Act
            Action act = () => _sut.IsValid(null, ValidAccountNumber);

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
            // Act
            Action act = () => _sut.IsValid(invalidSortCode, ValidAccountNumber);

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
            // Act
            Action act = () => _sut.IsValid(ValidSortCode, invalidAccountNumber);

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*accountNumber*");
        }

        [Fact]
        public void IsValid_ThrowsArgumentNullExceptionBecauseNullAccountNumberPassed()
        {
            // Act
            Action act = () => _sut.IsValid(ValidSortCode, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*input*");
        }

        [Fact]
        public void IsAccountNumberValid_ReturnsTrueAsNoModulusWeightFoundForSortCode()
        {
            // Arrange
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity>());

            // Act
            var result = _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsAccountNumberValid_ThrowsModulusCheckingExceptionDueAsTooManyWeightsFound()
        {
            // Arrange
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity> { new ModulusWeightEntity(), new ModulusWeightEntity(), new ModulusWeightEntity() });

            // Act
            Action act = () => _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            act.Should().ThrowExactly<ModulusCheckingException>().WithMessage("Expected no more than 2 modulus weights but received 3.");
        }

        [Fact]
        public void IsAccountNumberValid_ReturnsTrueAfterPassingOnlyRequiredCheck()
        {
            // Arrange
            var firstCheckEntity = new ModulusWeightEntity();
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity> { firstCheckEntity });
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, firstCheckEntity).Returns(true);

            // Act
            var result = _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("2")]
        [InlineData("9")]
        [InlineData("10")]
        [InlineData("11")]
        [InlineData("12")]
        [InlineData("13")]
        [InlineData("14")]
        public void IsAccountNumberValid_ReturnsTrueAfterPassingOnlyFirstCheckAndIgnoredExceptionFound(string modulusWeightException)
        {
            // Arrange
            var firstCheckEntity = new ModulusWeightEntity { ExceptionCode = modulusWeightException };
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity> { firstCheckEntity, new ModulusWeightEntity() });
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, firstCheckEntity).Returns(true);

            // Act
            var result = _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAccountNumberValid_ReturnsResultOfSecondCheckAfterPassingFirstCheck(bool secondCheckResult)
        {
            // Arrange
            var firstCheckEntity = new ModulusWeightEntity();
            var secondCheckEntity = new ModulusWeightEntity();
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity> { firstCheckEntity, secondCheckEntity });
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, firstCheckEntity).Returns(true);
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, secondCheckEntity).Returns(secondCheckResult);

            // Act
            var result = _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            result.Should().Be(secondCheckResult);
        }

        [Fact]
        public void IsAccountNumberValid_ReturnsFalseAfterFailingFirstCheckAndNoIgnoredExceptionFound()
        {
            // Arrange
            var firstCheckEntity = new ModulusWeightEntity { ExceptionCode = string.Empty };
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity> { firstCheckEntity });
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, firstCheckEntity).Returns(false);

            // Act
            var result = _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(true, "2")]
        [InlineData(true, "9")]
        [InlineData(true, "10")]
        [InlineData(true, "11")]
        [InlineData(true, "12")]
        [InlineData(true, "13")]
        [InlineData(true, "14")]
        public void IsAccountNumberValid_ReturnsResultOfSecondCheckAfterFailingFirstCheckAndIgnoredExceptionFound(bool secondCheckResult, string modulusWeightException)
        {
            // Arrange
            var firstCheckEntity = new ModulusWeightEntity { ExceptionCode = modulusWeightException };
            var secondCheckEntity = new ModulusWeightEntity();
            _modulusWeightRepository.GetForSortCode(ValidSortCodeAsInt).Returns(new List<ModulusWeightEntity> { firstCheckEntity, secondCheckEntity });
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, firstCheckEntity).Returns(false);
            _modulusCheckingService.IsValid(ValidSortCode, ValidAccountNumber, secondCheckEntity).Returns(true);

            // Act
            var result = _sut.IsValid(ValidSortCode, ValidAccountNumber);

            // Assert
            result.Should().Be(secondCheckResult);
        }

        #endregion        
    }
}
