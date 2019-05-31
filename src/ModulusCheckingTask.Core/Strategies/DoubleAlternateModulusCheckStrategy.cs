using System;
using System.Collections.Generic;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Strategies
{
    public class DoubleAlternateModulusCheckStrategy : ModulusCheckStrategyBase
    {
        #region Protected Methods

        protected override string GetStrategyMethodName() => "DBLAL";

        protected override bool IsValidModulusCheck(List<int> values, string accountNumber, ModulusWeightEntity modulusWeight)
        {
            var remainder = CalculateTotal(values) % 10;
            return remainder == 0;
        }

        #endregion

        #region Private Methods

        private int CalculateTotal(IEnumerable<int> values)
        {
            var total = 0;
            foreach (var value in values)
            {
                foreach (var digit in value.ToString())
                {
                    total += Convert.ToInt32(char.GetNumericValue(digit));
                }
            }

            return total;
        }

        #endregion
    }
}
