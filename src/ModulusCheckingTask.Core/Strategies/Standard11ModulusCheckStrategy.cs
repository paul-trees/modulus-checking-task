using System;
using System.Collections.Generic;
using System.Linq;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Strategies
{
    public class Standard11ModulusCheckStrategy : ModulusCheckStrategyBase
    {
        #region Protected Methods

        protected override string GetStrategyMethodName() => "MOD11";

        protected override bool IsValidModulusCheck(List<int> values, string accountNumber, ModulusWeightEntity modulusWeight)
        {
            if (modulusWeight.ExceptionCode == "7" && accountNumber.Substring(6, 1) == "9")
            {
                for (var loopIndex = 0; loopIndex < 8; loopIndex++)
                {
                    values[loopIndex] = 0;
                }
            }

            var remainder = values.Sum() % 11;

            if (modulusWeight.ExceptionCode == "4")
                return remainder == Convert.ToInt32(accountNumber.Substring(6, 2));

            return remainder == 0;
        }

        #endregion
    }
}
