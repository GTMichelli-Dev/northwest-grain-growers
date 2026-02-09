using GrainManagement.Models;

namespace GrainManagement.Services;

public interface IServerInfoProvider
{
    Task<Server> GetAsync(CancellationToken ct = default);
}