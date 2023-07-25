using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Primitives;
using eCommerce.Core.Services.Messages;
using eCommerce.Infrastructure.Extensions;

namespace eCommerce.Infrastructure.Services.Messages;

public class QueuedEmailService : IQueuedEmailService
{
    #region Fields

    private readonly IRepository<QueuedEmail> _queuedEmailRepository;

    #endregion

    #region Constructure and Destructure

    public QueuedEmailService(IRepository<QueuedEmail> queuedEmailRepository)
    {
        _queuedEmailRepository = queuedEmailRepository;
    }

    #endregion

    #region Public Methods

    public async Task InsertQueuedEmailAsync(QueuedEmail queuedEmail) => await _queuedEmailRepository.InsertAsync(queuedEmail);

    public async Task UpdateQueuedEmailAsync(QueuedEmail queuedEmail) => await _queuedEmailRepository.UpdateAsync(queuedEmail);

    public async Task DeleteQueuedEmailAsync(QueuedEmail queuedEmail) => await _queuedEmailRepository.DeleteAsync(queuedEmail);

    public async Task DeleteQueuedEmailsAsync(IList<QueuedEmail> queuedEmails) => await _queuedEmailRepository.DeleteAsync(queuedEmails);

    public async Task<QueuedEmail> GetQueuedEmailByIdAsync(Guid queuedEmailId) => await _queuedEmailRepository.GetByIdAsync(queuedEmailId);

    public async Task<IList<QueuedEmail>> GetQueuedEmailsByIdsAsync(Guid[] queuedEmailIds) => await _queuedEmailRepository.GetByIdsAsync(queuedEmailIds);

    public async Task<IPagedInfo<QueuedEmail>> SearchEmailsAsync(
        string fromEmail,
        string toEmail,
        DateTime? createdFromUtc,
        DateTime? createdToUtc,
        bool loadNotSentItemsOnly,
        bool loadOnlyItemsToBeSent,
        int maxSendTries,
        bool loadNewest,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        fromEmail = (fromEmail ?? string.Empty).Trim();
        toEmail = (toEmail ?? string.Empty).Trim();

        var query = _queuedEmailRepository.Table;

        if (!string.IsNullOrEmpty(fromEmail))
        {
            query = query.Where(qe => qe.From.Contains(fromEmail));
        }

        if (!string.IsNullOrEmpty(toEmail))
        {
            query = query.Where(qe => qe.To.Contains(toEmail));
        }

        if (createdFromUtc.HasValue)
        {
            query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
        }

        if (createdToUtc.HasValue)
        {
            query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);
        }

        if (loadNotSentItemsOnly)
        {
            query = query.Where(qe => !qe.SentOnUtc.HasValue);
        }

        query = query.Where(qe => qe.SentTries < maxSendTries);
        query = loadNewest ?
            query.OrderByDescending(qe => qe.CreatedOnUtc) :
            query.OrderByDescending(qe => qe.PriorityId).ThenBy(qe => qe.CreatedOnUtc);

        var queuedEmails = await query.ToPagedListAsync(pageIndex, pageSize);

        return queuedEmails;
    }

    public async Task<int> DeleteAlreadySentEmailsAsync(DateTime? createdFromUtc, DateTime? createdToUtc)
    {
        var query = _queuedEmailRepository.Table;

        query = query.Where(qe => qe.SentOnUtc.HasValue);

        if (createdFromUtc.HasValue)
        {
            query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
        }

        if (createdToUtc.HasValue)
        {
            query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);
        }

        var emails = query.ToArray();

        await DeleteQueuedEmailsAsync(emails);

        return emails.Length;
    }

    public async Task DeleteAllEmailsAsync() => await _queuedEmailRepository.TruncateAsync();

    #endregion
}