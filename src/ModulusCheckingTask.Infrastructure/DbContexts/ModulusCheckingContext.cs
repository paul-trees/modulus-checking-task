using Microsoft.EntityFrameworkCore;
using ModulusCheckingTask.Core.Entities;
using ModulusCheckingTask.Infrastructure.EntityConfigurations;

namespace ModulusCheckingTask.Infrastructure.DbContexts
{
    public class ModulusCheckingContext : DbContext
    {
        #region Constructors

        public ModulusCheckingContext()
        {

        }

        public ModulusCheckingContext(DbContextOptions<ModulusCheckingContext> options) : base(options)
        {

        }

        #endregion

        #region DbContext Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ModulusWeightEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region DbSets

        public DbSet<ModulusWeightEntity> ModulusWeights { get; set; }

        #endregion
    }
}
