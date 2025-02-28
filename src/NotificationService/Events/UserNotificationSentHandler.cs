using StudentPortal.EventBus;
using StudentPortal.Notifications.Hubs;
using StudentPortal.Notifications.Services;

namespace StudentPortal.Notifications.Events;

public record UserNotificationSent(Guid UserId, string Title, string Message, Dictionary<string, object> metadata) : IntegrationEvent;

public class UserNotificationSentHandler(ILogger<UserNotificationSent> logger, NotificationsService notificationsService, IEnumerable<INotificationsReceiver> receivers) : IIntegrationEventHandler<UserNotificationSent>
{
    public async Task Handle(UserNotificationSent @event)
    {
        var (userId, title, message, metadata) = @event;

        logger.LogTrace("Notification received. UserId: {UserId}", userId);

        var notification = await notificationsService.AddUserNotification(userId, title, message, metadata);

        foreach (var receiver in receivers)
        {
           await receiver.Receive(notification);
        }
    }
}