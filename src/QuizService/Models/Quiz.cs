using System.Collections.Immutable;
using StudentPortal.QuizService.DB.Entities;

namespace StudentPortal.QuizService.Models;

public record Quiz(Guid? Id, Guid QuizId, Guid UserId, ImmutableList<Quiz.QuizData> Data)
{
    public record QuizData(QuestionData Question, AnswerData Answer, ScoreData Score);
    public record QuestionData(string Text, string Type, string Data);
    public record AnswerData(string Text, string Data);
    public record ScoreData(double? Score, double MaxScore);
}

public static class QuizExtensions
{
    public static QuizData ToQuizData(this Quiz quiz)
    {
        return new QuizData()
        {
            Id = quiz.Id ?? Guid.Empty,
            QuizId = quiz.QuizId,
            UserId = quiz.UserId,
            Results = quiz.Data.Select(ToQuizResult).ToList()
        };
    }

    public static Quiz ToQuiz(this QuizData quiz)
    {
        return new Quiz(
            quiz.Id,
            quiz.QuizId,
            quiz.UserId,
            quiz.Results.Select(ToQuizData).ToImmutableList()
        );
    }

    public static Quiz.QuizData ToQuizData(this QuizResult quizResult)
    {
        return new Quiz.QuizData(
            Score: new(
                Score: quizResult.Score,
                MaxScore: quizResult.MaxScore
            ),
            Answer: new(
                Text: quizResult.Answer,
                Data: quizResult.AnswerData
            ),
            Question: new(
                Text: quizResult.QuestionText,
                Data: quizResult.QuestionData,
                Type: quizResult.QuestionType
            ));
    }

    public static QuizResult ToQuizResult(this Quiz.QuizData quizData)
    {
        return new QuizResult()
        {
            Answer = quizData.Answer.Text,
            AnswerData = quizData.Answer.Data,
            QuestionText = quizData.Question.Text,
            QuestionType = quizData.Question.Type,
            QuestionData = quizData.Question.Data,
            Score = quizData.Score.Score,
            MaxScore = quizData.Score.MaxScore
        };
    }
}