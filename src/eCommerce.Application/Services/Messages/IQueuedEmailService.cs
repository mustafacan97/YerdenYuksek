using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Primitives;

namespace eCommerce.Application.Services.Messages;

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
