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

        return quiz;
    }

    public async Task<IEnumerable<Quiz>> GetQuizResults(Guid quizId)
    {
        var data = await quizContext.QuizDatas.Where(x => x.QuizId == quizId).ToListAsync();

        return data.Select(QuizExtensions.ToQuiz);
    }
}