using StudentPortal.Services;
using StudentPortal.Services.DTO;

namespace StudentPortal.PageView;

public class SubmissionStrategy(IQuizService quizService, Guid pageId, Guid userId) : ISubmissionStrategy
{
    public async Task Submit(IEnumerable<QuizData> quizData)
    {
        await quizService.Publish(new (pageId, userId, quizData));
    }
}
