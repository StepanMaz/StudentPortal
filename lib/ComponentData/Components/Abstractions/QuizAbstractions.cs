using System.Collections.Immutable;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public abstract record QuizQuestionBase(Guid Id) : ComponentDataBase
{
    public abstract IQuestionDeclaration GetQuestionDeclaration();
}
