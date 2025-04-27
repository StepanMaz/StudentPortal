using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Auth;
using StudentPortal.Notifications.DTO;
using StudentPortal.Notifications.Services;

namespace StudentPortal.Notifications.Controllers;

[Authorize, ApiController, Route("/")]
public class NotificationsController(NotificationsService notificationsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<NotificationDTO>> GetNotifications()
    {
        if (!User.TryGetUserId(out var userId)) return Unauthorized();

        var notifications = await notificationsService.GetUserNotifications(userId);
        var res = notifications.Select(n => n.ToNotificationDTO());

        return Ok(res);
    }
}