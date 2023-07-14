﻿using eCommerce.Application.Models.Customers;
using eCommerce.Core.Domain.Security;
using eCommerce.Core.Primitives;
using YerdenYuksek.Application.Models.Customers;
using eCommerce.Core.Domain.Customers;

namespace eCommerce.Application.Services.Customers;

public partial interface ICustomerService
{
    #region Commands

    Task InsertCustomerAsync(Customer customer);

    Task<RegisterResponseModel> RegisterCustomerAsync(string email, string password);

    Task<Result> ValidateCustomerAsync(string email, string password);

    #endregion

    #region Queries

    Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false);

    Task<Role?> GetCustomerRoleByNameAsync(string name);

    #endregion
}