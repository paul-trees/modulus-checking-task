using System.Collections.Generic;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Strategies
{
    public interface IModulusCheckStrategy
    {
        bool IsApplicable(string methodName);
        bool IsValid(IEnumerable<int> resultsList, string accountNumber, ModulusWeightEntity modulusWeight);
    }
}
