using Microsoft.AspNetCore.Mvc;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Core.Primitives;
using YerdenYuksek.Web.Contract.Models.Customer;

namespace YerdenYuksek.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : Controller
{
    #region Fields

    private readonly ICustomerService _customerService;

    #endregion

    #region Constructure and Destructure

    public AuthenticationController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    #endregion

    #region Methods

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(q => q.Errors)
                .Select(x => Error.Validation(description: x.ErrorMessage));

            var result = Result.Failure(errors.ToList());

            return BadRequest(result);
        }

        var registerResult = await _customerService.RegisterCustomerAsync(model.Email, model.Password);
        return registerResult.IsSuccess ? Ok(registerResult) : BadRequest(registerResult);
    }

    #endregion
}
