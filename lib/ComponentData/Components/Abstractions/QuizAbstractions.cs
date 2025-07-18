using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public interface IQuestionDeclarationSource
{
    IQuestionDeclaration GetQuestionDeclaration();
}

public interface IQuestionDeclarationSource<out TQuestionDeclaration> where TQuestionDeclaration : IQuestionDeclaration
{
    TQuestionDeclaration GetQuestionDeclaration();
}

public abstract record QuizQuestionBase<T>(Guid Id) : ComponentBase(Id), IQuestionDeclarationSource, IQuestionDeclarationSource<T> where T : IQuestionDeclaration
{
    public abstract T GetQuestionDeclaration();

    IQuestionDeclaration IQuestionDeclarationSource.GetQuestionDeclaration()
    {
        return GetQuestionDeclaration();
    }
}
