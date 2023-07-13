using eCommerce.Core.Interfaces;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Messages;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class MessageTokenProvider : IMessageTokenProvider
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICustomerService _customerService;

    #endregion

    #region Constructure and Destructure

    public MessageTokenProvider(
        IUnitOfWork unitOfWork, ICustomerService customerService)
    {
        _unitOfWork = unitOfWork;
        _customerService = customerService;
    }

    #endregion

    #region Public Methods

    public async Task AddCustomerTokensAsync(IList<Token> tokens, Guid customerId)
    {
        var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(customerId);
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

    public IEnumerable<string> GetTokenGroups(MessageTemplate messageTemplate)
    {
        return messageTemplate.Name switch
        {
            MessageTemplateSystemNames.CustomerRegisteredStoreOwnerNotification or
            MessageTemplateSystemNames.CustomerWelcomeMessage or
            MessageTemplateSystemNames.CustomerEmailValidationMessage or
            MessageTemplateSystemNames.CustomerEmailRevalidationMessage or
            MessageTemplateSystemNames.CustomerPasswordRecoveryMessage => new[] { TokenGroupNames.CustomerTokens },            
            _ => Array.Empty<string>(),
        };
    }

    #endregion
}
