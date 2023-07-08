using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Messages;
using YerdenYuksek.Core.Primitives;

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
        await AddCustomerTokensAsync(tokens, customer);
    }

    public async Task AddCustomerTokensAsync(IList<Token> tokens, Customer customer)
    {
        tokens.Add(new Token("Customer.Email", customer.Email));
        tokens.Add(new Token("Customer.FullName", await _customerService.GetCustomerFullNameAsync(customer)));
        tokens.Add(new Token("Customer.FirstName", customer.FirstName));
        tokens.Add(new Token("Customer.LastName", customer.LastName));
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
