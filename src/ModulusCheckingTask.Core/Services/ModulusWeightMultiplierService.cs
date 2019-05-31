using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModulusCheckingTask.Core.Validation;

namespace ModulusCheckingTask.Core.Services
{
    public class ModulusWeightMultiplierService : IModulusWeightMultiplierService
    {
        #region IModulusWeightMultiplierService

        public IEnumerable<int> Execute(string combinedSortCodeAndAccountNumber, IEnumerable<int> modulusWeights)
        {
            if (!Regex.IsMatch(combinedSortCodeAndAccountNumber, ValidationRegex.CombinedSortCodeAndAccountNumber))
                throw new ArgumentException(nameof(combinedSortCodeAndAccountNumber));
            if (modulusWeights == null) throw new ArgumentNullException(nameof(modulusWeights));

            var modulusWeightsList = modulusWeights.ToList();
            if (combinedSortCodeAndAccountNumber.Length != modulusWeightsList.Count)
                throw new ArgumentException($"Expected number of digits in combined sort code and account number ({combinedSortCodeAndAccountNumber.Length}) to match the count of modulus weights ({modulusWeightsList.Count}).");

            return combinedSortCodeAndAccountNumber.Select((digit, loopIndex) => Convert.ToInt32(char.GetNumericValue(digit)) * modulusWeightsList[loopIndex]).ToList();
        }

        #endregion
    }
}
