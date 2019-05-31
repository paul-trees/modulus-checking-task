using System.Collections.Generic;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Adapters
{
    public interface IModulusWeightEntityAdapter
    {
        IEnumerable<int> Execute(string combinedSortCodeAndAccountNumber, ModulusWeightEntity modulusWeight);
    }
}
