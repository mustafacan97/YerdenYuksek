using eCommerce.Core.Entities.Messages;

namespace eCommerce.Core.Services.Messages;

public interface IEmailAccountService
{
    Task InsertEmailAccountAsync(EmailAccount emailAccount);

    Task UpdateEmailAccountAsync(EmailAccount emailAccount);

    Task DeleteEmailAccountAsync(EmailAccount emailAccount);

    Task<EmailAccount> GetEmailAccountByIdAsync(Guid emailAccountId);

    Task<IList<EmailAccount>> GetAllEmailAccountsAsync();
}
