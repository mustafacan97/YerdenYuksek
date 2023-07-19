using eCommerce.Core.Entities.Messages;

namespace eCommerce.Core.Services.Messages;

public interface IEmailSender
{
    Task SendEmailAsync(
        EmailAccount emailAccount,
        string subject,
        string body,
        string fromAddress,
        string fromName,
        string toAddress,
        string toName,
        string? replyToAddress = null,
        string? replyToName = null,
        IEnumerable<string>? bcc = null,
        IEnumerable<string>? cc = null,
        string? attachmentFilePath = null,
        string? attachmentFileName = null,
        int? attachedDownloadId = null,
        IDictionary<string, string>? headers = null);
}
