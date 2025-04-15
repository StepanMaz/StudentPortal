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
    T Visit(VarianceQuestion question);
    T Visit(OpenAnswerQuestion question);
}

public interface IAnswerVisitor<T>
{
    T Visit(VarianceQuestionAnswer answer, VarianceQuestion question);
    T Visit(OpenAnswerQuestionAnswer answer, OpenAnswerQuestion question);
}