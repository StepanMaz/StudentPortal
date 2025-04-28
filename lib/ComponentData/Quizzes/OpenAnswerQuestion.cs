namespace StudentPortal.ComponentData.Quizzes;

public record OpenAnswerQuestion(Guid Id, string QuestionText) : IQuestionDeclaration
{
    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor) => visitor.Visit(this);
}

public record OpenAnswerQuestionAnswer(OpenAnswerQuestion Question, string Answer) : IAnswer<OpenAnswerQuestion>
{
    public T Accept<T>(IAnswerVisitor<T> visitor) => visitor.Visit(this);
}
