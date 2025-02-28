namespace StudentPortal.ComponentData.Quizzes;

public interface IQuestionDeclaration
{
    T Accept<T>(IQuestionDeclarationVisitor<T> visitor);
}

public interface IAnswer<TQuestion>
{
    T Accept<T>(IAnswerVisitor<T> visitor, TQuestion question);
}

public interface IQuestionDeclarationVisitor<T>
{
    T Visit(SingleAnswerQuestion question);
    T Visit(MultiAnswerQuestion question);
    T Visit(OpenAnswerQuestion question);
}

public interface IAnswerVisitor<T>
{
    T Visit(SingleAnswerQuestionAnswer answer, SingleAnswerQuestion question);
    T Visit(MultiAnswerQuestionAnswer answer, MultiAnswerQuestion question);
    T Visit(OpenAnswerQuestionAnswer answer, OpenAnswerQuestion question);
}