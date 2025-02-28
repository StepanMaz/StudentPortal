using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public abstract record QuizQuestionBase : ComponentDataBase
{
    public Guid QuestionId { get; } = Guid.NewGuid();
    public abstract IQuestionDeclaration GetQuestionDeclaration();
}