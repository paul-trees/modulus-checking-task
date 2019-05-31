using System.Collections.Generic;

namespace ModulusCheckingTask.Core.Services
{
    public interface IModulusWeightMultiplierService
    {
        IEnumerable<int> Execute(string combinedSortCodeAndAccountNumber, IEnumerable<int> modulusWeightsList);
    }
}
