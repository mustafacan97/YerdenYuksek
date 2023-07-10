using Microsoft.AspNetCore.Mvc;
using YerdenYuksek.Application.Models.Customers;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : Controller
{
    #region Fields

    private readonly ICustomerService _customerService;

    private readonly IWorkflowMessageService _workflowMessageService;

    private readonly IWorkContext _workContext;

    #endregion

    #region Constructure and Destructure

    public AuthenticationController(
        ICustomerService customerService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService)
    {
        _customerService = customerService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
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
                    .ToList());

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

    #endregion
}
