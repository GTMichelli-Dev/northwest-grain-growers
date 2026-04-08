namespace GrainManagement.Services.Print;

public interface IPrintDispatchService
{
    Task DispatchTicketAsync(string ticketId, string? printerTarget = null, string type = "weighout", CancellationToken cancellationToken = default);
}

