using eCommerce.Core.Entities.Messages;

namespace eCommerce.Application.Services.Messages;

public interface IEmailTemplateService
{
    Task DeleteMessageTemplateAsync(EmailTemplate messageTemplate);

    Task InsertMessageTemplateAsync(EmailTemplate messageTemplate);

    Task UpdateMessageTemplateAsync(EmailTemplate messageTemplate);

    Task<EmailTemplate> GetMessageTemplateByIdAsync(Guid messageTemplateId);

    Task<IList<EmailTemplate>> GetMessageTemplatesByNameAsync(string messageTemplateName);

    Task<IList<EmailTemplate>> GetAllMessageTemplatesAsync(string? keywords = null, bool? isActive = null);
}
