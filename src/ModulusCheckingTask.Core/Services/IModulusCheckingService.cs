using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Core.Services
{
    public interface IModulusCheckingService
    {
        bool IsValid(string sortCode, string accountNumber, ModulusWeightEntity modulusWeight);
    }
}
