using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModulusCheckingTask.Core.DataTransferObjects;
using ModulusCheckingTask.Core.Services;

namespace ModulusCheckingTask.App.Controllers
{
    /// <summary>Provides services for validating account details.</summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailsValidationController : Controller
    {
        #region Fields

        private readonly IAccountDetailsValidationService _accountDetailsValidationService;

        #endregion

        #region Constructor

        public AccountDetailsValidationController(IAccountDetailsValidationService accountDetailsValidationService)
        {
            _accountDetailsValidationService = accountDetailsValidationService;
        }

        #endregion

        #region Actions

        /// <summary>Validates the provided account details.</summary>
        /// <param name="accountDetails"></param>
        /// <returns>A response to indicate whether or not the provided account details pass the modulus check.</returns>
        /// <response code="204">If the account details successfully pass the modulus check.</response>
        /// <response code="400">If the account details fail validation.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IsValid([FromBody] AccountDetailsValidationRequest accountDetails)
        {
            // TODO: Remove this line once the catch all handler is finished!
            if (accountDetails.SortCode.StartsWith("69")) throw new InvalidOperationException("Boom!");

            if (_accountDetailsValidationService.IsValid(accountDetails.SortCode, accountDetails.AccountNumber))
                return NoContent();
                
            return BadRequest(new ValidationProblemDetails
            {
                Errors = { new KeyValuePair<string, string[]>("ValidationFailure", new []{ "The provided sort code and account number values failed validation." }) },
                Status = StatusCodes.Status400BadRequest,
                Extensions = { new KeyValuePair<string, object>("traceId", HttpContext.TraceIdentifier) },
                Title = "A validation error occurred."
            });
        }

        #endregion
    }
}
