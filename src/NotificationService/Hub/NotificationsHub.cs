using Microsoft.AspNetCore.SignalR;
using StudentPortal.Notifications.DTO;
using StudentPortal.Notifications.Entities;

namespace StudentPortal.Notifications.Hubs;

public interface INotificationsReceiver
{
    Task Receive(Notification notification);
}

public class NotificationsHub : Hub, INotificationsReceiver
{
    private static readonly Dictionary<Guid, string> _userConnections = [];

    public async Task Receive(Notification notification)
    {
        var clientId = _userConnections[notification.UserId];

        await Clients.Client(clientId).SendAsync("ReceiveNotification", notification.ToNotificationDTO());
    }

    public async override Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", "Connected successfully.");

        if (Context.User is null) return;
        if (!Context.User.TryGetUserId(out var userId)) return;

        _userConnections.Add(userId, Context.ConnectionId);

        await Clients.Caller.SendAsync("ReceiveMessage", "Realtime notification receiver registered.");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User is null) return;
        if (!Context.User.TryGetUserId(out var userId)) return;

        _userConnections.Remove(userId);

        await base.OnDisconnectedAsync(exception);
    }
}