using eCommerce.Core.DomainEvents;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.Messages;
using MediatR;

namespace eCommerce.Application.Features.Events;

internal sealed class CustomerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
{
    #region Fields

    private readonly IWorkflowMessageService _workflowMessageService;

    private readonly IWorkContext _workContext;

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Constructure and Destructure

    public CustomerCreatedDomainEventHandler(
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        IUnitOfWork unitOfWork)
    {
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Public Methods

    public async Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(notification.CustomerId);

        if (customer is null)
        {
            return;
        }

        var currentLanguage = await _workContext.GetWorkingLanguageAsync();
        await _workflowMessageService.SendCustomerWelcomeMessageAsync(customer, currentLanguage.Id);
    }

    #endregion
}
