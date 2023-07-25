using eCommerce.Core.Entities.Configuration.CustomSettings;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Messages;
using eCommerce.Core.Shared;
using eCommerce.Core.Services.Messages;
using eCommerce.Core.Services.Localization;
using eCommerce.Core.Interfaces;

namespace eCommerce.Infrastructure.Services.Messages;

public class WorkflowMessageService : IWorkflowMessageService
{
    #region Fields

    private readonly EmailAccountSettings _emailAccountSettings;

    private readonly IRepository<Language> _languageRepository;

    private readonly IRepository<EmailAccount> _emailAccountRepository;

    private readonly IRepository<QueuedEmail> _queuedEmailRepository;

    private readonly IRepository<EmailTemplate> _emailTemplateRepository;

    private readonly ILocalizationService _localizationService;

    private readonly IMessageTokenProvider _messageTokenProvider;

    private readonly ITokenizer _tokenizer;

    #endregion

    #region Constructure and Destructure

    public WorkflowMessageService(
        IMessageTokenProvider messageTokenProvider,
        ILocalizationService localizationService,
        ITokenizer tokenizer,
        EmailAccountSettings emailAccountSettings,
        IRepository<Language> languageRepository,
        IRepository<EmailAccount> emailAccountRepository,
        IRepository<QueuedEmail> queuedEmailRepository,
        IRepository<EmailTemplate> emailTemplateRepository)
    {
        _messageTokenProvider = messageTokenProvider;
        _localizationService = localizationService;
        _tokenizer = tokenizer;
        _emailAccountSettings = emailAccountSettings;
        _languageRepository = languageRepository;
        _emailAccountRepository = emailAccountRepository;
        _queuedEmailRepository = queuedEmailRepository;
        _emailTemplateRepository = emailTemplateRepository;
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

        var messageTemplates = await _emailTemplateRepository.GetAllAsync(
            func: q => q.Where(p => p.Name == EmailTemplateSystemNames.CustomerWelcomeMessage),
            getCacheKey: q => q.PrepareKeyForDefaultCache(MessageDefaults.MessageTemplatesByNameCacheKey, EmailTemplateSystemNames.CustomerWelcomeMessage));

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
        var language = await _languageRepository.GetByIdAsync(languageId);

        if (language is null || !language.Active || language.Deleted)
        {
            language = await _languageRepository.GetFirstOrDefaultAsync(q => q.Where(p => p.IsDefaultLanguage));
        }

        if (language is null)
        {
            throw new Exception("No active language could be loaded");
        }

        return language.Id;
    }

    private async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(EmailTemplate messageTemplate)
    {
        var emailAccount =
            (await _emailAccountRepository.GetByIdAsync(messageTemplate.EmailAccountId) ?? await _emailAccountRepository.GetByIdAsync(_emailAccountSettings.DefaultEmailAccountId)) ??
            (await _emailAccountRepository.GetAllAsync()).FirstOrDefault();

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

        await _queuedEmailRepository.InsertAsync(email);
        return email.Id;
    }

    #endregion
}
