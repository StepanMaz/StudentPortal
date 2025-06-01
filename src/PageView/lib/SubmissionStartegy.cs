using System.Text.Json;
using StudentPortal.Services;
using StudentPortal.Services.DTO;

namespace StudentPortal.PageView;

public class SubmissionStrategy(IQuizService quizService, Guid pageId, Guid userId, Action<Quiz> OnSubmitted) : ISubmissionStrategy
{
    public async Task Submit(IEnumerable<QuizData> quizData)
    {
        var quiz = await quizService.Publish(new(pageId, userId, quizData));

        OnSubmitted?.Invoke(quiz);
    }
}
