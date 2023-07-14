using YerdenYuksek.Core.Domain.Customers;

namespace eCommerce.Application.Services.Messages;

public interface IWorkflowMessageService
{
    #region Customer workflow

    Task<IList<Guid>> SendCustomerWelcomeMessageAsync(Customer customer, Guid languageId);

    #endregion
}