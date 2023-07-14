using eCommerce.Core.Domain.Messages;
using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Application.Services.Messages;

public interface IMessageTokenProvider
{
    Task AddCustomerTokensAsync(IList<Token> tokens, Guid customerId);

    void AddCustomerTokens(IList<Token> tokens, Customer customer);

    IEnumerable<string> GetTokenGroups(EmailTemplate messageTemplate);
}