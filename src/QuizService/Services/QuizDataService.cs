using Microsoft.EntityFrameworkCore;

using StudentPortal.QuizService.DB;
using StudentPortal.QuizService.DB.Entities;
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

    public async Task<Quiz> UpdateQuizResult(Quiz quiz)
    {
        quizContext.Remove(new QuizData() { Id = quiz.Id!.Value });

        await quizContext.SaveChangesAsync();

        return await PublishQuizResult(quiz);
    }

    public async Task<Quiz?> GetQuizResults(Guid quizId)
    {
        var res = await quizContext.QuizDatas.Where(x => x.Id == quizId).FirstOrDefaultAsync();

        return res?.ToQuiz();
    }

    public async Task<IEnumerable<Quiz>> GetQuizzesAsync(Guid pageId)
    {
        var res = await quizContext.QuizDatas.Where(x => x.QuizId == pageId).ToListAsync();

        return res.Select(x => x.ToQuiz());
    }

    public async Task<IEnumerable<Quiz>> GetQuizzesByUser(Guid userId)
    {
        var res = await quizContext.QuizDatas.Where(x => x.UserId == userId).ToListAsync();

        return res.Select(x => x.ToQuiz());
    }
}