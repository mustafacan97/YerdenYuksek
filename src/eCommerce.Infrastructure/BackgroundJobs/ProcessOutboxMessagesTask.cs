using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Services.ScheduleTasks;
using MediatR;
using System.Text.Json;

namespace eCommerce.Infrastructure.BackgroundJobs;

public class ProcessOutboxMessagesTask : IScheduleTask
{
    #region Fields

    private readonly IRepository<OutboxMessage> _outboxMessageRepository;

    private readonly IPublisher _publisher;

    #endregion

    #region Constructure and Destructure

    public ProcessOutboxMessagesTask(
        IPublisher publisher, IRepository<OutboxMessage> outboxMessageRepository)
    {
        _publisher = publisher;
        _outboxMessageRepository = outboxMessageRepository;
    }

    #endregion

    #region Methods

    public async Task ExecuteAsync()
    {
        var messages = (await _outboxMessageRepository.GetAllAsync(q => q.Where(p => p.ProcessedOnUtc == null)));

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

        await _outboxMessageRepository.UpdateAsync(messages);
    }

    #endregion
}