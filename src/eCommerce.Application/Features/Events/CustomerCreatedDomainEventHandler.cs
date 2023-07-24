using eCommerce.Core.DomainEvents;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.Messages;
using MediatR;

namespace eCommerce.Application.Features.Events;

internal sealed class CustomerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
{
    #region Fields

    private readonly IRepository<Customer> _customerRepository;

    private readonly IWorkflowMessageService _workflowMessageService;

    private readonly IWorkContext _workContext;

    #endregion

    #region Constructure and Destructure

    public CustomerCreatedDomainEventHandler(
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        IRepository<Customer> customerRepository)
    {
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _customerRepository = customerRepository;
    }

    #endregion

    #region Public Methods

    public async Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);

        if (customer is null)
        {
            return;
        }

        var currentLanguage = await _workContext.GetWorkingLanguageAsync();
        await _workflowMessageService.SendCustomerWelcomeMessageAsync(customer, currentLanguage.Id);
    }

    #endregion
}
