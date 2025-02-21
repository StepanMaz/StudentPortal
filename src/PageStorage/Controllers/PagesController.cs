using Microsoft.AspNetCore.Mvc;
using StudentPortal.PageStorage.DTO;
using StudentPortal.PageStorage.Entities;
using StudentPortal.PageStorage.Services;

namespace StudentPortal.PageStorage.Controllers;

[ApiController]
public class PagesController(ILogger<PagesController> logger, PageService pageService) : ControllerBase
{
    [HttpPost("/")]
    public async Task<ActionResult<CreateNewPageResponse>> CreatePage([FromBody] CreateNewPageRequest request)
    {
        var page = await pageService.InsertPage(new Page()
        {
            Content = request.Content
        });

        logger.LogInformation("New page created. Id: {id}", page.Id);

        return Ok(new CreateNewPageResponse() { Id = page.Id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePage([FromRoute] Guid id)
    {
        await pageService.DeletePage(id);

        logger.LogInformation("Page {id} deleted", id);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPage([FromRoute] Guid id)
    {
        var page = await pageService.GetPage(id);

        return Ok(page);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePage([FromRoute] Guid id, [FromBody] UpdatePageDTO request)
    {
        await pageService.UpdatePage(id, content: request.Content, metadata: request.Metadata);

        logger.LogInformation("Page {id} updated", id);

        return Ok();
    }
}