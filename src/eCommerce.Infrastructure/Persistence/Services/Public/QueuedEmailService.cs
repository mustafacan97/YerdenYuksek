using eCommerce.Core.Interfaces;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Core.Domain.Messages;
using YerdenYuksek.Web.Framework.Persistence.Extensions;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class QueuedEmailService : IQueuedEmailService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public QueuedEmailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    public async Task InsertQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        await _unitOfWork.GetRepository<QueuedEmail>().InsertAsync(queuedEmail);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        _unitOfWork.GetRepository<QueuedEmail>().Update(queuedEmail);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        _unitOfWork.GetRepository<QueuedEmail>().Delete(queuedEmail);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteQueuedEmailsAsync(IList<QueuedEmail> queuedEmails)
    {
        foreach (var queuedEmail in queuedEmails)
        {
            _unitOfWork.GetRepository<QueuedEmail>().Delete(queuedEmail);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<QueuedEmail> GetQueuedEmailByIdAsync(Guid queuedEmailId)
    {
        return await _unitOfWork.GetRepository<QueuedEmail>().GetByIdAsync(queuedEmailId, cache => default);
    }

    public async Task<IList<QueuedEmail>> GetQueuedEmailsByIdsAsync(Guid[] queuedEmailIds)
    {
        return await _unitOfWork.GetRepository<QueuedEmail>().GetByIdsAsync(queuedEmailIds);
    }

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

        var query = _unitOfWork.GetRepository<QueuedEmail>().Table;
        if (!string.IsNullOrEmpty(fromEmail))
            query = query.Where(qe => qe.From.Contains(fromEmail));
        if (!string.IsNullOrEmpty(toEmail))
            query = query.Where(qe => qe.To.Contains(toEmail));
        if (createdFromUtc.HasValue)
            query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
        if (createdToUtc.HasValue)
            query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);
        if (loadNotSentItemsOnly)
            query = query.Where(qe => !qe.SentOnUtc.HasValue);

        query = query.Where(qe => qe.SentTries < maxSendTries);
        query = loadNewest ?
            query.OrderByDescending(qe => qe.CreatedOnUtc) :
            query.OrderByDescending(qe => qe.PriorityId).ThenBy(qe => qe.CreatedOnUtc);

        var queuedEmails = await query.ToPagedListAsync(pageIndex, pageSize);

        return queuedEmails;
    }

    public async Task<int> DeleteAlreadySentEmailsAsync(DateTime? createdFromUtc, DateTime? createdToUtc)
    {
        var query = _unitOfWork.GetRepository<QueuedEmail>().Table;

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

    public async Task DeleteAllEmailsAsync()
    {
        _unitOfWork.GetRepository<QueuedEmail>().Truncate();
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion
}