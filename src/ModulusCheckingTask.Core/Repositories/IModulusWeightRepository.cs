using System.Collections.Generic;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Repositories
{
    public interface IModulusWeightRepository
    {
        IEnumerable<ModulusWeightEntity> GetForSortCode(int sortCode);
        void Insert(ModulusWeightEntity modulusWeight);
        int Count();
        void SaveChanges();
    }
}
