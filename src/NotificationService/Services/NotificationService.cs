using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using StudentPortal.Notifications.DB;
using StudentPortal.Notifications.Entities;

namespace StudentPortal.Notifications.Services;

public class NotificationsService(ILogger<NotificationsService> logger, NotificationsContext context, IOptions<JsonOptions> options)
{
    public async Task<IEnumerable<Notification>> GetUserNotifications(Guid userId)
    {
        var notifications = await context.Notifications.Where(n => n.UserId == userId).ToListAsync();

        return notifications;
    }

    public async Task<Notification> AddUserNotification(Guid userId, string title, string message, Dictionary<string, object> metadata)
    {
        var notification = new Notification()
        {
            UserId = userId,
            Title = title,
            Message = message,
            Metadata = JsonSerializer.Serialize(metadata, options.Value.JsonSerializerOptions) 
        };


        await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();

        return notification;
    }
}