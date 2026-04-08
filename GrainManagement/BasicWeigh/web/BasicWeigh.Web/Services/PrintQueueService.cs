using System.Collections.Concurrent;

namespace BasicWeigh.Web.Services;

public class PrintQueueService
{
    private readonly ConcurrentQueue<string> _queue = new();
    private readonly ConcurrentDictionary<string, byte> _awaitingConfirmation = new();

    public void Enqueue(string ticketId) => _queue.Enqueue(ticketId);
    public bool TryDequeue(out string? ticketId) => _queue.TryDequeue(out ticketId);
    public bool HasPending => !_queue.IsEmpty;

    /// <summary>Mark a ticket as awaiting print confirmation (PDF fetch = confirmation).</summary>
    public void AwaitConfirmation(string ticketId) => _awaitingConfirmation.TryAdd(ticketId, 0);

    /// <summary>Check and clear confirmation. Returns true if the ticket was awaiting confirmation.</summary>
    public bool TryConfirm(string ticketId) => _awaitingConfirmation.TryRemove(ticketId, out _);
}
