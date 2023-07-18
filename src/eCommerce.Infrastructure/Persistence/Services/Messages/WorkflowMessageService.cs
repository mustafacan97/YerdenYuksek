using System.Linq.Dynamic.Core;
using eCommerce.Application.Services.Localization;
using eCommerce.Application.Services.Messages;
using eCommerce.Core.Helpers;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Messages;

namespace eCommerce.Infrastructure.Persistence.Services.Messages;

public class WorkflowMessageService : IWorkflowMessageService
{
    #region Fields

    private readonly EmailAccountSettings _emailAccountSettings;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ILanguageService _languageService;

    private readonly IEmailTemplateService _messageTemplateService;

    private readonly ILocalizationService _localizationService;

    private readonly IMessageTokenProvider _messageTokenProvider;

    private readonly ITokenizer _tokenizer;

    #endregion

    #region Constructure and Destructure

    public WorkflowMessageService(
        IUnitOfWork unitOfWork,
        ILanguageService languageService,
        IEmailTemplateService messageTemplateService,
        IMessageTokenProvider messageTokenProvider,
        ILocalizationService localizationService,
        ITokenizer tokenizer,
        EmailAccountSettings emailAccountSettings)
    {
        _unitOfWork = unitOfWork;
        _languageService = languageService;
        _messageTemplateService = messageTemplateService;
        _messageTokenProvider = messageTokenProvider;
        _localizationService = localizationService;
        _tokenizer = tokenizer;
        _emailAccountSettings = emailAccountSettings;
    }

    #endregion

    #region Public Methods

    public async Task<IList<Guid>> SendCustomerWelcomeMessageAsync(Customer customer, Guid languageId)
    {
        if (customer is null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        languageId = await EnsureLanguageIsActiveAsync(languageId);

        var messageTemplates = await GetActiveMessageTemplatesAsync(EmailTemplateSystemNames.CustomerWelcomeMessage);
        if (!messageTemplates.Any())
        {
            return new List<Guid>();
        }

        var commonTokens = new List<Token>();
        _messageTokenProvider.AddCustomerTokens(commonTokens, customer);

        return await messageTemplates.ToAsyncEnumerable().SelectAwait(async messageTemplate =>
        {
            var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate);
            var tokens = new List<Token>(commonTokens);
            var toEmail = customer.Email;
            var toName = $"{customer.FirstName} {customer.LastName}";

            return await SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
        }).ToListAsync();
    }

    #endregion

    #region Methods

    private async Task<Guid> EnsureLanguageIsActiveAsync(Guid languageId)
    {
        var language = await _unitOfWork.GetRepository<Language>().GetByIdAsync(languageId);

        if (language is null || !language.Active || language.Deleted)
        {
            language = await _languageService.GetDefaultLanguageAsync();
        }

        if (language is null)
        {
            throw new Exception("No active language could be loaded");
        }

        return language.Id;
    }

    private async Task<IList<EmailTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName)
    {
        var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(messageTemplateName);

        if (!messageTemplates?.Any() ?? true)
        {
            return new List<EmailTemplate>();
        }

        messageTemplates = messageTemplates.Where(mt => mt.Active && !mt.Deleted).ToList();

        return messageTemplates;
    }

    private async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(EmailTemplate messageTemplate)
    {
        var emailAccount =
            (await _unitOfWork.GetRepository<EmailAccount>().GetByIdAsync(messageTemplate.EmailAccountId) ?? await _unitOfWork.GetRepository<EmailAccount>().GetByIdAsync(_emailAccountSettings.DefaultEmailAccountId)) ??
            (await _unitOfWork.GetRepository<EmailAccount>().GetAllAsync()).FirstOrDefault();

        return emailAccount;
    }

    private async Task<Guid> SendNotificationAsync(
        EmailTemplate emailTemplate,
        EmailAccount emailAccount,
        Guid languageId,
        IList<Token> tokens,
        string toEmailAddress,
        string toName,
        string? attachmentFilePath = null,
        string? attachmentFileName = null,
        string? replyToEmailAddress = null,
        string? replyToName = null,
        string? fromEmail = null,
        string? fromName = null,
        string? subject = null)
    {
        if (emailTemplate is null)
        {
            throw new ArgumentNullException(nameof(emailTemplate));
        }

        if (emailAccount is null)
        {
            throw new ArgumentNullException(nameof(emailAccount));
        }

        var bcc = await _localizationService.GetLocalizedAsync(emailTemplate, et => et.Bcc, languageId);

        if (string.IsNullOrEmpty(subject))
        {
            subject = await _localizationService.GetLocalizedAsync(emailTemplate, et => et.Subject, languageId);
        }

        var body = await _localizationService.GetLocalizedAsync(emailTemplate, et => et.Body, languageId);

        //Replace subject and body tokens 
        var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
        var bodyReplaced = _tokenizer.Replace(body, tokens, true);

        //limit name length
        toName = CommonHelper.EnsureMaximumLength(toName, 300);

        var email = new QueuedEmail
        {
            Priority = QueuedEmailPriority.High,
            From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
            FromName = fromName ?? string.Empty,
            To = toEmailAddress,
            ToName = toName,
            ReplyTo = replyToEmailAddress,
            ReplyToName = replyToName,
            CC = string.Empty,
            Bcc = bcc,
            Subject = subjectReplaced,
            Body = bodyReplaced,
            CreatedOnUtc = DateTime.UtcNow,
            EmailAccountId = emailAccount.Id
        };

        await _unitOfWork.GetRepository<QueuedEmail>().InsertAsync(email);
        await _unitOfWork.SaveChangesAsync();
        return email.Id;
    }

    #endregion
}
