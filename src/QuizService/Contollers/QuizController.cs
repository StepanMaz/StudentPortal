using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Auth;
using StudentPortal.QuizService.Models;
using StudentPortal.QuizService.Services;

namespace StudentPortal.QuizService.Controllers;

[ApiController]
[Route("quiz")]
public class QuizController(QuizDataService quizService) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<Quiz>> PublishResults([FromBody] Quiz quizDTO)
    {
        return await quizService.PublishQuizResult(quizDTO);
    }

    [HttpPost]
    public async Task<ActionResult<Quiz>> UpdateResults([FromBody] Quiz quizDTO)
    {
        return await quizService.UpdateQuizResult(quizDTO);
    }

    [HttpGet("{testId}")]
    public async Task<IActionResult> GetTestQuizResults([FromRoute] Guid testId)
    {
        return Ok(await quizService.GetQuizResults(testId));
    }

    [HttpGet]
    public async Task<IActionResult> GetQuizzesByPage([FromQuery] Guid pageId)
    {
        return Ok(await quizService.GetQuizzesAsync(pageId));
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetQuizzesByUser()
    {
        if (HttpContext.User.TryGetUserId(out var id))
            return Ok(await quizService.GetQuizzesByUser(id));
        return Ok(new List<Quiz>());
    }
}