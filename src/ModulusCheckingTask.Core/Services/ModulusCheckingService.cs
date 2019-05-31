using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModulusCheckingTask.Core.Adapters;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Strategies;
using ModulusCheckingTask.Core.Validation;

namespace ModulusCheckingTask.Core.Services
{
    public class ModulusCheckingService : IModulusCheckingService
    {
        #region Fields

        private readonly List<IModulusCheckStrategy> _modulusCheckingStrategies;
        private readonly IModulusWeightEntityAdapter _modulusWeightEntityAdapter; 

        #endregion

        #region Constructor

        public ModulusCheckingService(IEnumerable<IModulusCheckStrategy> modulusCheckingStrategies, IModulusWeightEntityAdapter modulusWeightEntityAdapter)
        {
            _modulusCheckingStrategies = new List<IModulusCheckStrategy>(modulusCheckingStrategies);
            _modulusWeightEntityAdapter = modulusWeightEntityAdapter;
        }

        #endregion

        #region IModulusCheckingService

        public bool IsValid(string sortCode, string accountNumber, ModulusWeightEntity modulusWeight)
        {
            if (!Regex.IsMatch(sortCode, ValidationRegex.SortCode)) throw new ArgumentException(nameof(sortCode));
            if (!Regex.IsMatch(accountNumber, ValidationRegex.AccountNumber)) throw new ArgumentException(nameof(accountNumber));
            if (modulusWeight == null) throw new ArgumentNullException(nameof(modulusWeight));

            var resultsList = _modulusWeightEntityAdapter.Execute(sortCode + accountNumber, modulusWeight);
            return _modulusCheckingStrategies.Single(s => s.IsApplicable(modulusWeight.ModCheck)).IsValid(resultsList, accountNumber, modulusWeight);
        }

        #endregion        
    }
}
