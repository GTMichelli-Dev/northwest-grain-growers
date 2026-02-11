using GrainManagement.Dtos.Scales;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs;

public interface IScaleClient
{
    Task ScaleUpdated(ScaleDto dto);
}

public sealed class ScaleHub : Hub<IScaleClient>
{
    public static string GroupName(int scaleId) => $"scale:{scaleId}";

    public Task JoinScale(int scaleId)
        => Groups.AddToGroupAsync(Context.ConnectionId, GroupName(scaleId));

    public Task LeaveScale(int scaleId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(scaleId));
}
