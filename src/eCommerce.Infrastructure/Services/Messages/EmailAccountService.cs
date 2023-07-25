using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.Messages;
using eCommerce.Core.Shared;

namespace eCommerce.Infrastructure.Services.Messages;

public class EmailAccountService : IEmailAccountService
{
    #region Fields

    private readonly IRepository<EmailAccount> _emailAccountsRepository;

    #endregion

    #region Constructure and Destructure

    public EmailAccountService(IRepository<EmailAccount> emailAccountsRepository)
    {
        _emailAccountsRepository = emailAccountsRepository;
    }

    #endregion

    #region Methods

    public async Task InsertEmailAccountAsync(EmailAccount emailAccount) =>  await _emailAccountsRepository.InsertAsync(emailAccount);

    public async Task UpdateEmailAccountAsync(EmailAccount emailAccount) => await _emailAccountsRepository.UpdateAsync(emailAccount);

    public async Task DeleteEmailAccountAsync(EmailAccount emailAccount)
    {
        if (emailAccount is null)
        {
            throw new ArgumentNullException(nameof(emailAccount));
        }

        if ((await GetAllEmailAccountsAsync()).Count == 1)
        {
            throw new Exception("You cannot delete this email account. At least one account is required.");
        }

        await _emailAccountsRepository.DeleteAsync(emailAccount);
    }

    public async Task<EmailAccount> GetEmailAccountByIdAsync(Guid emailAccountId) => await _emailAccountsRepository.GetByIdAsync(emailAccountId);

    public async Task<IList<EmailAccount>> GetAllEmailAccountsAsync()
    {
        return await _emailAccountsRepository.GetAllAsync(getCacheKey: q => q.PrepareKeyForDefaultCache(EntityCacheDefaults<EmailAccount>.AllCacheKey));
    }

    #endregion
}