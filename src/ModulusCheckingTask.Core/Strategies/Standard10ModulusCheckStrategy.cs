using System.Collections.Generic;
using System.Linq;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Strategies
{
    public class Standard10ModulusCheckStrategy : ModulusCheckStrategyBase
    {
        #region Protected Methods

        protected override string GetStrategyMethodName() => "MOD10";

        protected override bool IsValidModulusCheck(List<int> values, string accountNumber, ModulusWeightEntity modulusWeight)
        {
            var remainder = values.Sum() % 10;
            return remainder == 0;
        }

        #endregion
    }
}
