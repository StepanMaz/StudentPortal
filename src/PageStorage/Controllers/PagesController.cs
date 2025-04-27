using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Auth;
using StudentPortal.ComponentData;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.PageStorage.DTO;
using StudentPortal.PageStorage.Entities;
using StudentPortal.PageStorage.Services;

namespace StudentPortal.PageStorage.Controllers;

[ApiController]
[Route("")]
public class PagesController(ILogger<PagesController> logger, PageService pageService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Page>> CreatePage([FromBody] CreateNewPageRequest request)
    {
        if (!HttpContext.User.TryGetUserId(out var userId)) return Unauthorized();

        var page = await pageService.InsertPage(new Page()
        {
            Name = request.Name,
            OwnerId = userId,
            Content = request.Content,
            Metadata = request.Metadata
        });

        logger.LogInformation("New page created. Id: {id}", page.Id);

        return Ok(page);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePage([FromRoute] Guid id)
    {
        if (!HttpContext.User.TryGetUserId(out var userId)) return Unauthorized();

        await pageService.DeletePage(id, userId);

        logger.LogInformation("Page {id} deleted", id);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPage([FromRoute] Guid id)
    {
        var page = await pageService.GetPage(id);

        return Ok(page);
    }

    [HttpGet]
    public async Task<ActionResult> GetPage([FromQuery] string key, [FromQuery] Guid userId)
    {
        var page = await pageService.GetPageByKey(userId, key);

        return Ok(page);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePage([FromRoute] Guid id, [FromBody] UpdatePageDTO request)
    {
        if (!HttpContext.User.TryGetUserId(out var userId)) return Unauthorized();

        await pageService.UpdatePage(new Page()
        {
            Id = id,
            OwnerId = userId,
            Content = request.Content,
            Name = request.Name,
            Metadata = request.Metadata
        });

        logger.LogInformation("Page {id} updated", id);

        return Ok();
    }

    [Authorize]
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<FileInfoDTO>>> List()
    {
        if (!HttpContext.User.TryGetUserId(out var userId)) return Unauthorized();

        var pages = await pageService.GetUserPages(userId);

        return Ok(pages.Select(p => new FileInfoDTO() { Id = p.Id.ToString(), Name = p.Name }));
    }
}