using eCommerce.Core.Interfaces;
using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Services.Messages;

namespace eCommerce.Infrastructure.Services.Messages;

public class EmailTemplateService : IEmailTemplateService
{
    # region Fields

    private readonly IRepository<EmailTemplate> _emailTemplateRepository;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructure and Destructure

    public EmailTemplateService(
        IStaticCacheManager staticCacheManager,
        IRepository<EmailTemplate> emailTemplateRepository)
    {
        _staticCacheManager = staticCacheManager;
        _emailTemplateRepository = emailTemplateRepository;
    }

    #endregion

    #region Public Methods

    public async Task DeleteMessageTemplateAsync(EmailTemplate messageTemplate) => await _emailTemplateRepository.DeleteAsync(messageTemplate);

    public async Task<IList<EmailTemplate>> GetAllMessageTemplatesAsync(string? keywords = null, bool? isActive = null)
    {
        var messageTemplates = await _emailTemplateRepository.GetAllAsync(query =>
        {
            query = query.Where(q => !q.Deleted);

            if (isActive.HasValue)
            {
                query = query.Where(mt => mt.Active == isActive);
            }

            return query.OrderBy(t => t.Name);
        }, cache => cache.PrepareKeyForDefaultCache(MessageDefaults.MessageTemplatesAllCacheKey, isActive));

        if (!string.IsNullOrWhiteSpace(keywords))
        {
            messageTemplates = messageTemplates.Where(x => (x.Subject?.Contains(keywords, StringComparison.InvariantCultureIgnoreCase) ?? false)
                || (x.Body?.Contains(keywords, StringComparison.InvariantCultureIgnoreCase) ?? false)
                || (x.Name?.Contains(keywords, StringComparison.InvariantCultureIgnoreCase) ?? false)).ToList();
        }

        return messageTemplates;
    }

    public async Task<EmailTemplate> GetMessageTemplateByIdAsync(Guid messageTemplateId) => await _emailTemplateRepository.GetByIdAsync(messageTemplateId);

    public async Task<IList<EmailTemplate>> GetMessageTemplatesByNameAsync(string messageTemplateName)
    {
        if (string.IsNullOrWhiteSpace(messageTemplateName))
        {
            throw new ArgumentException(nameof(messageTemplateName));
        }

        return await _emailTemplateRepository.GetAllAsync(
            func: q => q.Where(p => p.Name == messageTemplateName),
            getCacheKey: q => q.PrepareKeyForDefaultCache(MessageDefaults.MessageTemplatesByNameCacheKey, messageTemplateName));
    }

    public async Task InsertMessageTemplateAsync(EmailTemplate messageTemplate) => await _emailTemplateRepository.InsertAsync(messageTemplate);

    public async Task UpdateMessageTemplateAsync(EmailTemplate messageTemplate) => await _emailTemplateRepository.UpdateAsync(messageTemplate);

    #endregion
}
