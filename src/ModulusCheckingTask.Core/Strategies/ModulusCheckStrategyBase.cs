using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Validation;

namespace ModulusCheckingTask.Core.Strategies
{
    public abstract class ModulusCheckStrategyBase : IModulusCheckStrategy
    {
        #region IModulusCheckStrategy

        public bool IsApplicable(string methodName)
        {
            return methodName == GetStrategyMethodName();
        }

        public bool IsValid(IEnumerable<int> resultsList, string accountNumber, ModulusWeightEntity modulusWeight)
        {
            if (!Regex.IsMatch(accountNumber, ValidationRegex.AccountNumber)) throw new ArgumentException(nameof(accountNumber));            
            if (resultsList == null) throw new ArgumentNullException(nameof(resultsList));
            if (modulusWeight == null) throw new ArgumentNullException(nameof(modulusWeight));
            if (!IsApplicable(modulusWeight.ModCheck)) throw new ArgumentException($"Check applicable to provided {nameof(ModulusWeightEntity)} does not match {GetStrategyMethodName()}.");

            return IsValidModulusCheck(resultsList.ToList(), accountNumber, modulusWeight);
        }

        #endregion

        #region Protected Methods

        protected abstract bool IsValidModulusCheck(List<int> values, string accountNumber, ModulusWeightEntity modulusWeight);

        protected abstract string GetStrategyMethodName();

        #endregion
    }
}
