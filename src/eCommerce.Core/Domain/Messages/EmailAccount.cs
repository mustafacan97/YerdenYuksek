using eCommerce.Core.Primitives;

namespace eCommerce.Core.Domain.Messages;

public class EmailAccount : SoftDeletedEntity
{
    #region Constructure and Destructure

    public EmailAccount()
    {
        EmailTemplates = new HashSet<EmailTemplate>();
        QueuedEmails = new HashSet<QueuedEmail>();
    }

    #endregion

    #region Public Properties

    public string Email { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public bool EnableSsl { get; set; }

    public ICollection<EmailTemplate> EmailTemplates { get; set; }

    public ICollection<QueuedEmail> QueuedEmails { get; set; }

    #endregion
}
