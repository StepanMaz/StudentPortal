using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public record OpenAnswerQuestionComponent(Guid Id, string QuestionText, double MaxScore) : QuizQuestionBase<OpenAnswerQuestion>(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);

    public override OpenAnswerQuestion GetQuestionDeclaration()
    {
        return new OpenAnswerQuestion(
            Id: Id,
            QuestionText: this.QuestionText,
            MaxScore: MaxScore
        );
    }
}
