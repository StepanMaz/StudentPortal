namespace StudentPortal.ComponentData.Quizzes;

public interface IQuestionDeclaration
{
    public Guid Id { get; }
    public string QuestionText { get; }
    T Accept<T>(IQuestionDeclarationVisitor<T> visitor);
}


public interface IAnswer<out TQuestion> where TQuestion : IQuestionDeclaration
{
    TQuestion Question { get; }
    T Accept<T>(IAnswerVisitor<T> visitor);
}

public interface IQuestionDeclarationVisitor<T>
{
    T Visit(VarianceQuestion question);
    T Visit(OpenAnswerQuestion question);
}

public interface IAnswerVisitor<T>
{
    T Visit(VarianceQuestionAnswer answer);
    T Visit(OpenAnswerQuestionAnswer answer);
}

public interface IQuizWriter
{
    public Task Set(IAnswer<IQuestionDeclaration> answer);
    public Task Submit();
}

public interface IQuizManager : IQuizWriter
{
    public Task<IAnswer<IQuestionDeclaration>?> Get(IQuestionDeclaration question);
}

public interface IQuizScoreCalculator 
{
    public double? GetScore(IAnswer<IQuestionDeclaration> answer);
}