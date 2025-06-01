using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{testId}")]
    public async Task<IActionResult> GetTestQuizResults([FromRoute] Guid testId)
    {
        return Ok(await quizService.GetQuizResults(testId));
    }
}