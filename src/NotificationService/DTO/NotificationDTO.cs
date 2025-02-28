using StudentPortal.Notifications.Entities;

namespace StudentPortal.Notifications.DTO;

public class NotificationDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Message { get; set; }

    public bool Read { get; set; }
    public bool Acknowledged { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }

    public string? Metadata { get; set; }
}

public static class NotificationDTOMapperExtensions
{
    public static NotificationDTO ToNotificationDTO(this Notification notification)
    {
        return new NotificationDTO()
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            Read = notification.Status != NotificationStatus.Unread,
            Acknowledged = notification.Status == NotificationStatus.Acknowledged,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt,
            Metadata = notification.Metadata
        };
    }
}