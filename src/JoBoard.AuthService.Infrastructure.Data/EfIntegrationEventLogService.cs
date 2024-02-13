using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Application.Common.Services;

namespace JoBoard.AuthService.Infrastructure.Data;

public class EfIntegrationEventLogService : IIntegrationEventLogService, IDisposable
{
    public Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
    {
        throw new NotImplementedException();
    }

    public Task SaveEventAsync(IntegrationEvent @event)
    {
        throw new NotImplementedException();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}