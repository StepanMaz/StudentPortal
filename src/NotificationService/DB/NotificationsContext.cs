using Microsoft.EntityFrameworkCore;
using StudentPortal.Notifications.Entities;

namespace StudentPortal.Notifications.DB;

public class NotificationsContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }

    public NotificationsContext(DbContextOptions<NotificationsContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}