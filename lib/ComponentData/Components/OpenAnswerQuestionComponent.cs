using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public record OpenAnswerQuestionComponent(string QuestionText) : QuizQuestionBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);

    public override IQuestionDeclaration GetQuestionDeclaration()
    {
        return new OpenAnswerQuestion(
            Id: this.QuestionId,
            QuestionText: this.QuestionText
        );
    }
}
