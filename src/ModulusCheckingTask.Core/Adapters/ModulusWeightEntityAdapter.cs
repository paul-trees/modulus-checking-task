using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Services;
using ModulusCheckingTask.Core.Validation;

namespace ModulusCheckingTask.Core.Adapters
{
    public class ModulusWeightEntityAdapter : IModulusWeightEntityAdapter
    {
        #region Fields

        private readonly IModulusWeightMultiplierService _modulusWeightMultiplierService;

        #endregion

        #region Constructor

        public ModulusWeightEntityAdapter(IModulusWeightMultiplierService modulusWeightMultiplierService)
        {
            _modulusWeightMultiplierService = modulusWeightMultiplierService;
        }

        #endregion

        #region IModulusWeightEntityAdapter

        public IEnumerable<int> Execute(string combinedSortCodeAndAccountNumber, ModulusWeightEntity modulusWeight)
        {
            if (!Regex.IsMatch(combinedSortCodeAndAccountNumber, ValidationRegex.CombinedSortCodeAndAccountNumber))
                throw new ArgumentException(nameof(combinedSortCodeAndAccountNumber));
            if (modulusWeight == null) throw new ArgumentNullException(nameof(modulusWeight));

            var modulusWeightsList = modulusWeight.GetType().GetProperties().Where(p => p.Name.StartsWith("Weight")).Select(
                p =>
                {
                    var value = p.GetValue(modulusWeight);
                    return (int)value;
                }).ToList();

            return _modulusWeightMultiplierService.Execute(combinedSortCodeAndAccountNumber, modulusWeightsList);
        }

        #endregion
    }
}
