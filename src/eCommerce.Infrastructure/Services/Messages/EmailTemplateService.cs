using eCommerce.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Services.Caching;
using eCommerce.Core.Services.Messages;

namespace eCommerce.Infrastructure.Services.Messages;

public class EmailTemplateService : IEmailTemplateService
{
    # region Fields

    private readonly IUnitOfWork _unitOfWork;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructure and Destructure

    public EmailTemplateService(
        IUnitOfWork unitOfWork,
        IStaticCacheManager staticCacheManager)
    {
        _unitOfWork = unitOfWork;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Public Methods

    public async Task DeleteMessageTemplateAsync(EmailTemplate messageTemplate)
    {
        _unitOfWork.GetRepository<EmailTemplate>().Delete(messageTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IList<EmailTemplate>> GetAllMessageTemplatesAsync(string? keywords = null, bool? isActive = null)
    {
        var messageTemplates = await _unitOfWork.GetRepository<EmailTemplate>().GetAllAsync(query =>
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

    public async Task<EmailTemplate> GetMessageTemplateByIdAsync(Guid messageTemplateId)
    {
        return await _unitOfWork.GetRepository<EmailTemplate>().GetByIdAsync(messageTemplateId);
    }

    public async Task<IList<EmailTemplate>> GetMessageTemplatesByNameAsync(string messageTemplateName)
    {
        if (string.IsNullOrWhiteSpace(messageTemplateName))
        {
            throw new ArgumentException(nameof(messageTemplateName));
        }

        var key = _staticCacheManager.PrepareKeyForDefaultCache(MessageDefaults.MessageTemplatesByNameCacheKey, messageTemplateName);

        return await _staticCacheManager.GetAsync(key, async () =>
        {
            var templates = await _unitOfWork.GetRepository<EmailTemplate>().Table
                .Where(messageTemplate => messageTemplate.Name.Equals(messageTemplateName))
                .OrderBy(messageTemplate => messageTemplate.Id)
                .ToListAsync();

            return templates;
        });
    }

    public async Task InsertMessageTemplateAsync(EmailTemplate messageTemplate)
    {
        await _unitOfWork.GetRepository<EmailTemplate>().InsertAsync(messageTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateMessageTemplateAsync(EmailTemplate messageTemplate)
    {
        _unitOfWork.GetRepository<EmailTemplate>().Update(messageTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion
}
