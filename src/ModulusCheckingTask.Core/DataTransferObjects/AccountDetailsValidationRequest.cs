using System.ComponentModel.DataAnnotations;
using ModulusCheckingTask.Core.Validation;

namespace ModulusCheckingTask.Core.DataTransferObjects
{
    public class AccountDetailsValidationRequest
    {
        [RegularExpression(ValidationRegex.SortCode, ErrorMessage = "{0} must be provided and be a 6 digit number.")]
        public string SortCode { get; set; }

        [RegularExpression(ValidationRegex.AccountNumber, ErrorMessage = "{0} must be provided and be a 8 digit number.")]
        public string AccountNumber { get; set; }
    }
}
