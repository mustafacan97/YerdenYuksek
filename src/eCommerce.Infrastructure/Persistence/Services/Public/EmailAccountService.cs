using eCommerce.Application.Services.Messages;
using eCommerce.Core.Domain.Messages;
using eCommerce.Core.Interfaces;

namespace eCommerce.Infrastructure.Persistence.Services.Messages;

public class EmailAccountService : IEmailAccountService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public EmailAccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task InsertEmailAccountAsync(EmailAccount emailAccount)
    {
        if (emailAccount is null)
        {
            throw new ArgumentNullException(nameof(emailAccount));
        }

        await _unitOfWork.GetRepository<EmailAccount>().InsertAsync(emailAccount);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateEmailAccountAsync(EmailAccount emailAccount)
    {
        if (emailAccount is null)
        {
            throw new ArgumentNullException(nameof(emailAccount));
        }

        _unitOfWork.GetRepository<EmailAccount>().Update(emailAccount);
        await _unitOfWork.SaveChangesAsync();
    }

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

        _unitOfWork.GetRepository<EmailAccount>().Delete(emailAccount);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<EmailAccount> GetEmailAccountByIdAsync(Guid emailAccountId)
    {
        return await _unitOfWork.GetRepository<EmailAccount>().GetByIdAsync(emailAccountId, cache => default);
    }

    public async Task<IList<EmailAccount>> GetAllEmailAccountsAsync()
    {
        var emailAccounts = await _unitOfWork.GetRepository<EmailAccount>().GetAllAsync(query =>
        {
            return from ea in query
                   orderby ea.Id
                   select ea;
        }, cache => default);

        return emailAccounts;
    }

    #endregion
}