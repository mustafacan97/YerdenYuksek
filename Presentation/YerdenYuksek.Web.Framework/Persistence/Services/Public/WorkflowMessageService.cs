using System.Linq.Dynamic.Core;
using YerdenYuksek.Application.Services.Public.Customers;
using YerdenYuksek.Application.Services.Public.Localization;
using YerdenYuksek.Application.Services.Public.Messages;
using YerdenYuksek.Core;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Core.Domain.Localization;
using YerdenYuksek.Core.Domain.Messages;
using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Web.Framework.Persistence.Services.Public;

public class WorkflowMessageService : IWorkflowMessageService
{
    #region Fields

    private readonly EmailAccountSettings _emailAccountSettings;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ILanguageService _languageService;

    private readonly IMessageTemplateService _messageTemplateService;

    private readonly IMessageTokenProvider _messageTokenProvider;

    private readonly ILocalizationService _localizationService;

    private readonly ICustomerService _customerService;

    private readonly ITokenizer _tokenizer;

    #endregion

    #region Constructure and Destructure

    public WorkflowMessageService(
        IUnitOfWork unitOfWork, ILanguageService languageService, IMessageTemplateService messageTemplateService, IMessageTokenProvider messageTokenProvider, ILocalizationService localizationService, ICustomerService customerService, EmailAccountSettings emailAccountSettings, ITokenizer tokenizer)
    {
        _unitOfWork = unitOfWork;
        _languageService = languageService;
        _messageTemplateService = messageTemplateService;
        _messageTokenProvider = messageTokenProvider;
        _localizationService = localizationService;
        _customerService = customerService;
        _emailAccountSettings = emailAccountSettings;
        _tokenizer = tokenizer;
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

        var messageTemplates = await GetActiveMessageTemplatesAsync(MessageTemplateSystemNames.CustomerWelcomeMessage);
        if (!messageTemplates.Any())
        {
            return new List<Guid>();
        }
        
        var commonTokens = new List<Token>();
        await _messageTokenProvider.AddCustomerTokensAsync(commonTokens, customer);

        return await messageTemplates.ToAsyncEnumerable().SelectAwait(async messageTemplate =>
        {
            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate);

            var tokens = new List<Token>(commonTokens);


            var toEmail = customer.Email;
            var toName = _customerService.GetCustomerFullName(customer);

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

    private async Task<IList<MessageTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName)
    {
        var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(messageTemplateName);
        
        if (!messageTemplates?.Any() ?? true)
        {
            return new List<MessageTemplate>();
        }

        messageTemplates = messageTemplates.Where(mt => mt.Active && !mt.Deleted).ToList();

        return messageTemplates;
    }

    private async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate)
    {
        var emailAccount = 
            (await _unitOfWork.GetRepository<EmailAccount>().GetByIdAsync(messageTemplate.EmailAccountId) ?? await _unitOfWork.GetRepository<EmailAccount>().GetByIdAsync(EmailAccountSettings.DefaultEmailAccountId)) ??
            (await _unitOfWork.GetRepository<EmailAccount>().GetAllAsync()).FirstOrDefault();
        
        return emailAccount;
    }

    private async Task<Guid> SendNotificationAsync(
        MessageTemplate messageTemplate,
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
        if (messageTemplate is null)
        {
            throw new ArgumentNullException(nameof(messageTemplate));
        }

        if (emailAccount is null)
        {
            throw new ArgumentNullException(nameof(emailAccount));
        }
        
        var bcc = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.BccEmailAddresses, languageId);

        if (string.IsNullOrEmpty(subject))
        {
            subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
        }

        var body = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Body, languageId);

        //Replace subject and body tokens 
        var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
        var bodyReplaced = _tokenizer.Replace(body, tokens, true);

        //limit name length
        toName = CommonHelper.EnsureMaximumLength(toName, 300);

        var email = new QueuedEmail
        {
            Priority = QueuedEmailPriority.High,
            From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
            FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
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
