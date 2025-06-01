using Microsoft.EntityFrameworkCore;

using StudentPortal.QuizService.DB;
using StudentPortal.QuizService.Models;

namespace StudentPortal.QuizService.Services;

public class QuizDataService(QuizContext quizContext)
{
    public async Task<Quiz> PublishQuizResult(Quiz quiz)
    {
        var quizData = quiz.ToQuizData();

        quizData.UpdatedAt = DateTime.UtcNow;

        quizContext.QuizDatas.Add(quizData);

        await quizContext.SaveChangesAsync();

        return quizData.ToQuiz();
    }

    public async Task<Quiz?> GetQuizResults(Guid quizId)
    {
        var res = await quizContext.QuizDatas.Where(x => x.Id == quizId).FirstOrDefaultAsync();

        return res?.ToQuiz();
    }
}