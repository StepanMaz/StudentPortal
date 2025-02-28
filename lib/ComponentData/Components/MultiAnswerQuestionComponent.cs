using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public record MultiAnswerQuestionComponent(
    string QuestionText,
    ImmutableList<MultiAnswerQuestionComponent.AnswerData> AnswerDatas,
    ImmutableList<Guid> CorrectAnswersIds,
    bool Shuffle) : QuizQuestionBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);

    public override IQuestionDeclaration GetQuestionDeclaration()
    {
        var variants = AnswerDatas.Select(ad => ad.ToVariant()).ToImmutableList();
        var answer = variants.Where(v => CorrectAnswersIds.Contains(v.Id)).ToImmutableList();

        return new MultiAnswerQuestion(
            Id: this.QuestionId,
            QuestionText: this.QuestionText,
            Variants: variants,
            Answers: answer
        );
    }

    public record AnswerData(string AnswerText)
    {
        public Guid Id { get; } = Guid.NewGuid();

        public MultiAnswerQuestion.Variant ToVariant()
        {
            return new MultiAnswerQuestion.Variant(Id, AnswerText);
        }
    }
}
