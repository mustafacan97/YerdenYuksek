using YerdenYuksek.Core.Domain.Customers;

namespace YerdenYuksek.Application.Services.Public.Messages;

public interface IWorkflowMessageService
{
    #region Customer workflow

    Task<IList<Guid>> SendCustomerWelcomeMessageAsync(Customer customer, Guid languageId);

    #endregion
}