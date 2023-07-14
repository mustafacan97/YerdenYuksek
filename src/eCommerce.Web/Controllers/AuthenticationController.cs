using eCommerce.Application.Models.Customers;
using eCommerce.Application.Services.Customers;
using eCommerce.Application.Services.Messages;
using eCommerce.Application.Services.Security;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YerdenYuksek.Application.Models.Customers;

namespace YerdenYuksek.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : Controller
{
    #region Fields

    private readonly ICustomerService _customerService;

    private readonly IWorkflowMessageService _workflowMessageService;

    private readonly IWorkContext _workContext;

    private readonly IJwtService _jwtService;

    #endregion

    #region Constructure and Destructure

    public AuthenticationController(
        ICustomerService customerService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        IJwtService jwtService)
    {
        _customerService = customerService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _jwtService = jwtService;
    }

    #endregion

    #region Methods

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = Result.Failure(
                ModelState.Values
                    .SelectMany(q => q.Errors)
                    .Select(x => Error.Validation(description: x.ErrorMessage))
                    .ToArray());

            return BadRequest(result);
        }

        var registerResult = await _customerService.RegisterCustomerAsync(model.Email, model.Password);

        if (registerResult.IsSuccess)
        {
            var currentLanguage = await _workContext.GetWorkingLanguageAsync();
            await _workflowMessageService.SendCustomerWelcomeMessageAsync(registerResult.Customer, currentLanguage.Id);
            return Ok();
        }
        else
        {
            return BadRequest(registerResult);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = Result.Failure(
                ModelState.Values
                    .SelectMany(q => q.Errors)
                    .Select(x => Error.Validation(description: x.ErrorMessage))
                    .ToArray());

            return BadRequest(result);
        }

        var loginResult = await _customerService.ValidateCustomerAsync(model.Email, model.Password);
        if (!loginResult.IsSuccess)
        {
            return Ok(loginResult);
        }

        var token = _jwtService.GenerateJwtToken(model.Email);
        if (token is null)
        {
            return Ok(Result.Failure(Error.Unexpected(description: "Unexpected error occurred!")));
        }

        return Ok(token);
    }

    #endregion
}
