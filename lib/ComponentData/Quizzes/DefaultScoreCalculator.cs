namespace StudentPortal.ComponentData.Quizzes;

public class DefaultScoreCalculator : IQuizScoreCalculator, IAnswerVisitor<double?>
{
    public static readonly DefaultScoreCalculator Instance = new DefaultScoreCalculator();
    public double? GetScore(IAnswer<IQuestionDeclaration> answer)
    {
        return answer.Accept(this);
    }

    public double? Visit(VarianceQuestionAnswer answer)
    {
        var (question, answers) = answer;

        var correctAnswersCount = answers.Union(question.Answers).Count();
        var totalAnswersCount = answers.Count;
        var wrongAnswersCount = Math.Max(correctAnswersCount - totalAnswersCount, 0);

        return question.Answers.Count switch
        {
            0 => question.MaxScore,
            1 => correctAnswersCount != 0 ? question.MaxScore : 0,
            _ => (correctAnswersCount - wrongAnswersCount) / (double)question.Answers.Count * question.MaxScore
        };
    }

    public double? Visit(OpenAnswerQuestionAnswer answer)
    {
        return null;
    }
}


public static class AnswerScoreExtensions
{
    public static double? GetScore(this IAnswer<IQuestionDeclaration> answer)
    {
        return answer.Accept(DefaultScoreCalculator.Instance);
    }
}