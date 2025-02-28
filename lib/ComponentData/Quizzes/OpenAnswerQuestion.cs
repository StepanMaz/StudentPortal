namespace StudentPortal.ComponentData.Quizzes;

public record OpenAnswerQuestion(Guid Id, string QuestionText) : IQuestionDeclaration
{
    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor) => visitor.Visit(this);
}

public record OpenAnswerQuestionAnswer(string Answer) : IAnswer<OpenAnswerQuestion>
{
    public T Accept<T>(IAnswerVisitor<T> visitor, OpenAnswerQuestion question) => visitor.Visit(this, question);
}
