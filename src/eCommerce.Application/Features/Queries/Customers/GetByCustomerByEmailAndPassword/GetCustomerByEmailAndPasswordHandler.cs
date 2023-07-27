using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Security;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Customers;
using eCommerce.Core.Shared;
using Mapster;
using MediatR;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmailAndPassword;

public class GetCustomerByEmailAndPasswordHandler : IRequestHandler<GetCustomerByEmailAndPasswordQuery, Result<GetCustomerByEmailAndPasswordResponse>>
{
    #region Fields

    private readonly CustomerSettings _customerSettings;

    private readonly ICustomerService _customerService;

    private readonly IWebHelper _webHelper;

    #endregion

    #region Constructure and Destructure

    public GetCustomerByEmailAndPasswordHandler(
        CustomerSettings customerSettings,
        IWebHelper webHelper,
        ICustomerService customerService)
    {
        _customerSettings = customerSettings;
        _webHelper = webHelper;
        _customerService = customerService;
    }

    #endregion

    #region Public Methods

    public async Task<Result<GetCustomerByEmailAndPasswordResponse>> Handle(
        GetCustomerByEmailAndPasswordQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerByEmailAsync(request.Email);

        if (customer is null)
        {
            return Result<GetCustomerByEmailAndPasswordResponse>.NotFound(Error.NotFound(description: "Customer not found!"));
        }

        if (!customer.Active || customer.Deleted)
        {
            return Result<GetCustomerByEmailAndPasswordResponse>.NotFound(Error.Failure(description: "Customer must be active!"));
        }

        // Only registered can login
        if (!customer.CustomerHasSpecifiedRole(RoleDefaults.RegisteredRoleName))
        {
            return Result<GetCustomerByEmailAndPasswordResponse>.Forbidden(Error.Failure(description: "Customer must be registered!"));
        }

        var maxFailedLoginAttempts = _customerSettings.FailedPasswordAllowedAttempts;

        //check whether a customer is locked out
        if (customer.CustomerHasLoginRestrictions())
        {
            return Result<GetCustomerByEmailAndPasswordResponse>.Forbidden(Error.Failure(description: "Account is locked out!"));
        }

        if (!PasswordsMatch(customer.CustomerSecurity, request.Password))
        {
            customer.CustomerSecurity.FailedLoginAttempts++;
            if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                customer.CustomerSecurity.FailedLoginAttempts >= maxFailedLoginAttempts)
            {
                customer.LockOutCustomerAccount(_customerSettings.FailedPasswordLockoutMinutes);
            }

            await _customerService.UpdateCustomerAsync(customer);

            return Result<GetCustomerByEmailAndPasswordResponse>.Failure(Error.Failure(description: "Email or password is not correect!"));
        }

        customer.CustomerSecurity.FailedLoginAttempts = 0;
        customer.CustomerSecurity.CannotLoginUntilDateUtc = null;
        customer.CustomerSecurity.RequireReLogin = false;
        customer.CustomerSecurity.LastIpAddress = _webHelper.GetCurrentIpAddress();
        customer.LastLoginDateUtc = DateTime.UtcNow;

        await _customerService.UpdateCustomerAsync(customer);

        var result = customer.Adapt<GetCustomerByEmailAndPasswordResponse>();

        return Result<GetCustomerByEmailAndPasswordResponse>.Success(result);
    }

    #endregion

    #region Methods

    private static bool PasswordsMatch(CustomerSecurity customerSecurity, string enteredPassword)
    {
        if (customerSecurity is null || string.IsNullOrEmpty(enteredPassword))
        {
            return false;
        }

        if (customerSecurity.Password is null)
        {
            return false;
        }

        var savedPassword = EncryptionHelper.CreatePasswordHash(enteredPassword, customerSecurity.PasswordSalt, "SHA512");

        return customerSecurity.Password.Equals(savedPassword);
    }

    #endregion
}
