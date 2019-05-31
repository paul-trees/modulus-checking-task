using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ModulusCheckingTask.Core.Adapters;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Services;
using NSubstitute;
using Xunit;

namespace ModulusCheckingTask.Core.UnitTests.Adapters
{
    public class ModulusWeightEntityAdapterTests
    {
        #region Fields

        private readonly IModulusWeightMultiplierService _modulusWeightMultiplierService;
        private readonly ModulusWeightEntityAdapter _sut;

        #endregion

        #region Constants

        private const string CombinedSortCodeAndAccountNumber = "23456789123456";

        #endregion

        #region Constructor

        public ModulusWeightEntityAdapterTests()
        {
            _modulusWeightMultiplierService = Substitute.For<IModulusWeightMultiplierService>();
            _sut = new ModulusWeightEntityAdapter(_modulusWeightMultiplierService);
        }

        #endregion

        #region Tests

        [Theory]
        [InlineData("1234567890123")]
        [InlineData("123456789012345")]
        [InlineData("1234567890123A")]
        [InlineData("")]
        public void Execute_ThrowsArgumentExceptionAsInvalidCombinedSortCodeAndAccountNumberProvided(string combinedSortCodeAndAccountNumber)
        {
            // Act
            Action act = () => _sut.Execute(combinedSortCodeAndAccountNumber, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("*combinedSortCodeAndAccountNumber*");
        }

        [Fact]
        public void Execute_ThrowsArgumentNullExceptionWhenNullCombinedSortCodeAndAccountNumberProvided()
        {
            // Act
            Action act = () => _sut.Execute(null, new ModulusWeightEntity());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*input*");
        }

        [Fact]
        public void GetModulusWeightsAsList_ThrowsArgumentNullExceptionWhenNullModulusWeightEntityProvided()
        {
            // Act
            Action act = () => _sut.Execute(CombinedSortCodeAndAccountNumber, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*modulusWeight*");
        }

        [Fact]
        public void GetModulusWeightsAsList_ExecutesSuccessfully()
        {
            // Assert
            var modulusWeight = CreateTestModulusWeightEntity();
            var resultList = new List<int> { 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            _modulusWeightMultiplierService.Execute(CombinedSortCodeAndAccountNumber, Arg.Any<List<int>>()).Returns(resultList);

            // Act
            var result = _sut.Execute(CombinedSortCodeAndAccountNumber, modulusWeight);

            // Assert
            result.Should().ContainInOrder(resultList);
            _modulusWeightMultiplierService.Received(1).Execute(CombinedSortCodeAndAccountNumber, 
                Arg.Is<List<int>>(l => l.SequenceEqual(new[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})));
        }

        #endregion

        #region Test Helpers

        private static ModulusWeightEntity CreateTestModulusWeightEntity()
        {
            return new ModulusWeightEntity
            {
                WeightU = 1,
                WeightV = 2,
                WeightW = 3,
                WeightX = 4,
                WeightY = 5,
                WeightZ = 6,
                WeightA = 7,
                WeightB = 8,
                WeightC = 9,
                WeightD = 10,
                WeightE = 11,
                WeightF = 12,
                WeightG = 13,
                WeightH = 14
            };
        }

        #endregion
    }
}
