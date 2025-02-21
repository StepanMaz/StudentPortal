using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentPortal.Notifications.Controllers;

[Authorize, ApiController, Route("/")]
public class NotificationsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<object>> GetNotifications()
    {
        return Ok(User.Identity.Name);
    }
}