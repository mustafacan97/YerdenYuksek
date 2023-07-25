using eCommerce.Core.Interfaces;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Services.Messages;

namespace eCommerce.Infrastructure.Services.Messages;

public class MessageTokenProvider : IMessageTokenProvider
{
    #region Fields

    private readonly IRepository<Customer> _customerRepository;

    #endregion

    #region Constructure and Destructure

    public MessageTokenProvider(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    #endregion

    #region Public Methods

    public async Task AddCustomerTokensAsync(IList<Token> tokens, Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        AddCustomerTokens(tokens, customer);
    }

    public void AddCustomerTokens(IList<Token> tokens, Customer customer)
    {
        tokens.Add(new Token("Customer.Email", customer.Email));

        if (!string.IsNullOrEmpty(customer.FirstName))
        {
            tokens.Add(new Token("Customer.FirstName", customer.FirstName));
        }

        if (!string.IsNullOrEmpty(customer.LastName))
        {
            tokens.Add(new Token("Customer.LastName", customer.LastName));
        }

        if (!string.IsNullOrWhiteSpace(customer.FirstName) && !string.IsNullOrWhiteSpace(customer.LastName))
        {
            tokens.Add(new Token("Customer.FullName", $"{customer.FirstName} {customer.LastName}"));
        }
    }

    public IEnumerable<string> GetTokenGroups(EmailTemplate emailTemplate)
    {
        return emailTemplate.Name switch
        {
            EmailTemplateSystemNames.CustomerRegisteredStoreOwnerNotification or
            EmailTemplateSystemNames.CustomerWelcomeMessage or
            EmailTemplateSystemNames.CustomerEmailValidationMessage or
            EmailTemplateSystemNames.CustomerEmailRevalidationMessage or
            EmailTemplateSystemNames.CustomerPasswordRecoveryMessage => new[] { TokenGroupNames.CustomerTokens },
            _ => Array.Empty<string>(),
        };
    }

    #endregion
}