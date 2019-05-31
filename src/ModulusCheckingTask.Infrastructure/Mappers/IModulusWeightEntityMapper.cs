using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Infrastructure.Mappers
{
    public interface IModulusWeightEntityMapper
    {
        ModulusWeightEntity Create(string modulusWeightData);
    }
}
