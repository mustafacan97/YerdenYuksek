using eCommerce.Core.Interfaces;
using YerdenYuksek.Core.Domain.Messages;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Application.Services.Public.Messages;

public interface IQueuedEmailService
{
    Task InsertQueuedEmailAsync(QueuedEmail queuedEmail);

    Task UpdateQueuedEmailAsync(QueuedEmail queuedEmail);

    Task DeleteQueuedEmailAsync(QueuedEmail queuedEmail);

    Task DeleteQueuedEmailsAsync(IList<QueuedEmail> queuedEmails);

    Task<QueuedEmail> GetQueuedEmailByIdAsync(Guid queuedEmailId);

    Task<IList<QueuedEmail>> GetQueuedEmailsByIdsAsync(Guid[] queuedEmailIds);

    Task<IPagedInfo<QueuedEmail>> SearchEmailsAsync(
        string fromEmail,
        string toEmail,
        DateTime? createdFromUtc,
        DateTime? createdToUtc,
        bool loadNotSentItemsOnly,
        bool loadOnlyItemsToBeSent,
        int maxSendTries,
        bool loadNewest,
        int pageIndex = 0,
        int pageSize = int.MaxValue);

    Task<int> DeleteAlreadySentEmailsAsync(DateTime? createdFromUtc, DateTime? createdToUtc);

    Task DeleteAllEmailsAsync();
}
