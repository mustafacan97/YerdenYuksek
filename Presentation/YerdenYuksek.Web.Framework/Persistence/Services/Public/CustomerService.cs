using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Domain.Configuration;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class CustomerService : ICustomerService
{
    #region Fields

    private readonly IRepository<Customer> _customerRepository;

    #endregion

    #region Constructure and Destructure

    public CustomerService(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    #endregion

    #region Public Methods

    public async Task<bool> IsRegisteredAsync(string email)
    {
        var customer = await _customerRepository.GetAllAsync(query =>
        {
            return from c in query
                   where c.Email == email
                   select c;
        });

        return customer is not null;
    }

    #endregion
}
