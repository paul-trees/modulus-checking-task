using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Core.Repositories;
using ModulusCheckingTask.Infrastructure.DbContexts;

namespace ModulusCheckingTask.Infrastructure.Repositories
{
    public class ModulusWeightRepository : IModulusWeightRepository
    {
        #region Fields

        private readonly ModulusCheckingContext _dbContext;

        #endregion

        #region Constructor

        public ModulusWeightRepository(ModulusCheckingContext modulusCheckingContext)
        {
            _dbContext = modulusCheckingContext;
        }

        #endregion

        #region IModulusWeightRepository

        public IEnumerable<ModulusWeightEntity> GetForSortCode(int sortCode)
        {
            return _dbContext.ModulusWeights
                .Where(mw => mw.SortCodeRangeStart <= sortCode && mw.SortCodeRangeEnd >= sortCode)
                .AsNoTracking()
                .ToList();
        }

        public void Insert(ModulusWeightEntity modulusWeight)
        {
            if (modulusWeight == null) throw new ArgumentNullException(nameof(modulusWeight));

            _dbContext.ModulusWeights.Add(modulusWeight);
        }

        public int Count()
        {
            return _dbContext.ModulusWeights.Count();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        #endregion
    }
}
