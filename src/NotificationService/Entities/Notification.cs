namespace StudentPortal.Notifications.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }

    public required string Title { get; set; }
    public required string Message { get; set; }


    public NotificationType NotificationType { get; set; } = NotificationType.App;
    public NotificationStatus Status { get; set; } = NotificationStatus.Unread;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }

    public string? Metadata { get; set; }
}

public enum NotificationType
{
    App
}

public enum NotificationStatus
{
    Unread,
    Read
}
