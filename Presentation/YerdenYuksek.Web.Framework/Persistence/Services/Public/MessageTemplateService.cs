using Microsoft.EntityFrameworkCore;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Core.Caching;
using YerdenYuksek.Core.Domain.Messages;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class MessageTemplateService : IMessageTemplateService
{
    # region Fields

    private readonly IUnitOfWork _unitOfWork;

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Constructure and Destructure

    public MessageTemplateService(
        IUnitOfWork unitOfWork, 
        IStaticCacheManager staticCacheManager)
    {
        _unitOfWork = unitOfWork;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Public Methods

    public async Task DeleteMessageTemplateAsync(MessageTemplate messageTemplate)
    {
        _unitOfWork.GetRepository<MessageTemplate>().Delete(messageTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IList<MessageTemplate>> GetAllMessageTemplatesAsync(string? keywords = null, bool? isActive = null)
    {
        var messageTemplates = await _unitOfWork.GetRepository<MessageTemplate>().GetAllAsync(query =>
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

    public async Task<MessageTemplate> GetMessageTemplateByIdAsync(Guid messageTemplateId)
    {
        return await _unitOfWork.GetRepository<MessageTemplate>().GetByIdAsync(messageTemplateId, cache => default);
    }

    public async Task<IList<MessageTemplate>> GetMessageTemplatesByNameAsync(string messageTemplateName)
    {
        if (string.IsNullOrWhiteSpace(messageTemplateName))
        {
            throw new ArgumentException(nameof(messageTemplateName));
        }

        var key = _staticCacheManager.PrepareKeyForDefaultCache(MessageDefaults.MessageTemplatesByNameCacheKey, messageTemplateName);

        return await _staticCacheManager.GetAsync(key, async () =>
        {            
            var templates = await _unitOfWork.GetRepository<MessageTemplate>().Table
                .Where(messageTemplate => messageTemplate.Name.Equals(messageTemplateName))
                .OrderBy(messageTemplate => messageTemplate.Id)
                .ToListAsync();

            return templates;
        });
    }

    public async Task InsertMessageTemplateAsync(MessageTemplate messageTemplate)
    {
        await _unitOfWork.GetRepository<MessageTemplate>().InsertAsync(messageTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateMessageTemplateAsync(MessageTemplate messageTemplate)
    {
        _unitOfWork.GetRepository<MessageTemplate>().Update(messageTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion
}
