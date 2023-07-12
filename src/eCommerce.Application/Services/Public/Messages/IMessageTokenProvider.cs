using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Messages;

namespace YerdenYuksek.Application.Services.Public.Messages;

public interface IMessageTokenProvider
{
    Task AddCustomerTokensAsync(IList<Token> tokens, Guid customerId);

    Task AddCustomerTokensAsync(IList<Token> tokens, Customer customer);

    IEnumerable<string> GetTokenGroups(MessageTemplate messageTemplate);
}