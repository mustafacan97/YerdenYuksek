using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.ScheduleTasks;
using MediatR;
using System.Text.Json;

namespace eCommerce.Infrastructure.BackgroundJobs;

public class ProcessOutboxMessagesTask : IScheduleTask
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    private readonly IPublisher _publisher;

    #endregion

    #region Constructure and Destructure

    public ProcessOutboxMessagesTask(
        IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    #endregion

    #region Methods

    public async Task ExecuteAsync()
    {
        var messages = (await _unitOfWork.GetRepository<OutboxMessage>().GetAllAsync(q =>
        {
            return from s in q
                   where s.ProcessedOnUtc == null
                   select s;
        }));

        if (messages is null || !messages.Any())
        {
            return;
        }

        foreach(OutboxMessage message in messages)
        {
            var domainEvent = JsonSerializer.Deserialize(message.Content, Type.GetType(message.Type));

            if (domainEvent is null)
            {
                continue;
            }

            await _publisher.Publish(domainEvent);

            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    #endregion
}