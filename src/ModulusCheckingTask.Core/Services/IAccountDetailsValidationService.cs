namespace ModulusCheckingTask.Core.Services
{
    public interface IAccountDetailsValidationService
    {
        bool IsValid(string sortCode, string accountNumber);
    }
}
