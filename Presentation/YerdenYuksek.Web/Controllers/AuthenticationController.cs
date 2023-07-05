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
    public async Task<Result> Register(RegisterModel model)
    {
        var registerResult = await _customerService.RegisterCustomerAsync(model.Email, model.Password);
        return registerResult;
    }

    #endregion
}
