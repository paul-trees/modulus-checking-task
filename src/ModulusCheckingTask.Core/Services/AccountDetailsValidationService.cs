using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModulusCheckingTask.Core.Exceptions;
using ModulusCheckingTask.Core.Repositories;
using ModulusCheckingTask.Core.Validation;

namespace ModulusCheckingTask.Core.Services
{
    public class AccountDetailsValidationService : IAccountDetailsValidationService
    {
        #region Fields

        private readonly List<string> _ignoredExceptions = new List<string> { "2", "5", "9", "10", "11", "12", "13", "14" };
        private readonly IModulusWeightRepository _modulusWeightRepository;
        private readonly IModulusCheckingService _modulusCheckingService;

        #endregion

        #region Constructor

        public AccountDetailsValidationService(IModulusWeightRepository modulusWeightRepository, IModulusCheckingService modulusCheckingService)
        {
            _modulusWeightRepository = modulusWeightRepository;
            _modulusCheckingService = modulusCheckingService;
        }

        #endregion

        #region IModulusCheckingService

        public bool IsValid(string sortCode, string accountNumber)
        {
            if (!Regex.IsMatch(sortCode, ValidationRegex.SortCode)) throw new ArgumentException(nameof(sortCode));
            if (!Regex.IsMatch(accountNumber, ValidationRegex.AccountNumber)) throw new ArgumentException(nameof(accountNumber));

            var modulusWeights = _modulusWeightRepository.GetForSortCode(int.Parse(sortCode)).ToList();

            // If there is no modulus weight for the sort code then we have to assume the account number is correct
            if (!modulusWeights.Any()) return true;

            if (modulusWeights.Count > 2) throw new ModulusCheckingException($"Expected no more than 2 modulus weights but received {modulusWeights.Count}.");

            var modulusWeight = modulusWeights.First();
            var firstCheckResult = _modulusCheckingService.IsValid(sortCode, accountNumber, modulusWeight);
            if (firstCheckResult && (modulusWeights.Count == 1 || _ignoredExceptions.Contains(modulusWeight.ExceptionCode)))
                return true;
            if (!firstCheckResult && !_ignoredExceptions.Contains(modulusWeight.ExceptionCode))
                return false;

            modulusWeight = modulusWeights.Last();
            return _modulusCheckingService.IsValid(sortCode, accountNumber, modulusWeight);
        }

        #endregion
    }
}
