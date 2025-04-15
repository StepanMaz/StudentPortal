using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.ComponentData.Components;

public record VarianceQuestionComponent(
    Guid Id,
    string QuestionText,
    ImmutableList<VarianceQuestionComponent.VarianceAnswer> Variants,
    ImmutableHashSet<Guid> CorrectAnswersIds,
    bool Shuffle) : QuizQuestionBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);

    public override IQuestionDeclaration GetQuestionDeclaration()
    {
        var variants = Variants.Select(ad => new VarianceQuestion.Variant(ad.Id, ad.AnswerText)).ToImmutableList();
        var answer = variants.Where(v => CorrectAnswersIds.Contains(v.Id)).ToImmutableList();

        return new VarianceQuestion(
            Id: this.Id,
            QuestionText: this.QuestionText,
            Variants: variants,
            Answers: answer
        );
    }

    public record VarianceAnswer(Guid Id, string AnswerText);
}
