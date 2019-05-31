using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Infrastructure.EntityConfigurations
{
    public class ModulusWeightEntityConfiguration : IEntityTypeConfiguration<ModulusWeightEntity>
    {
        #region IEntityTypeConfiguration

        public void Configure(EntityTypeBuilder<ModulusWeightEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }

        #endregion
    }
}
