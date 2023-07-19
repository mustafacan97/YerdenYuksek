using eCommerce.Core.Entities.Customers;

namespace eCommerce.Core.Services.Messages;

public interface IWorkflowMessageService
{
    #region Customer workflow

    Task<IList<Guid>> SendCustomerWelcomeMessageAsync(Customer customer, Guid languageId);

    #endregion
}