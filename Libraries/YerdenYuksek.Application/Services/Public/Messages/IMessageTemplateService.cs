using YerdenYuksek.Core.Domain.Messages;

namespace YerdenYuksek.Application.Services.Public.Messages;

public interface IMessageTemplateService
{
    Task DeleteMessageTemplateAsync(MessageTemplate messageTemplate);

    Task InsertMessageTemplateAsync(MessageTemplate messageTemplate);

    Task UpdateMessageTemplateAsync(MessageTemplate messageTemplate);

    Task<MessageTemplate> GetMessageTemplateByIdAsync(Guid messageTemplateId);

    Task<IList<MessageTemplate>> GetMessageTemplatesByNameAsync(string messageTemplateName);

    Task<IList<MessageTemplate>> GetAllMessageTemplatesAsync(string? keywords = null, bool? isActive = null);
}
