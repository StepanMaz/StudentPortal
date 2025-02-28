using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public record SingleAnswerQuestionComponent(
    string QuestionText,
    ImmutableList<SingleAnswerQuestionComponent.AnswerData> AnswerDatas,
    Guid CorrectAnswerId,
    bool Shuffle) : QuizQuestionBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);

    public override IQuestionDeclaration GetQuestionDeclaration()
    {
        var variants = AnswerDatas.Select(ad => ad.ToVariant()).ToImmutableList();
        var answer = variants.First(v => v.Id == CorrectAnswerId);

        return new SingleAnswerQuestion(
            Id: this.QuestionId,
            QuestionText: this.QuestionText,
            Variants: variants,
            Answer: answer
        );
    }

    public record AnswerData(string AnswerText)
    {
        public Guid Id { get; } = Guid.NewGuid();

        public SingleAnswerQuestion.Variant ToVariant()
        {
            return new SingleAnswerQuestion.Variant(Id, AnswerText);
        }
    }
}
