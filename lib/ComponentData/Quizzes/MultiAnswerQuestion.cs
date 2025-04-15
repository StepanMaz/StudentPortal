using System.Collections.Immutable;

namespace StudentPortal.ComponentData.Quizzes;

using Variants = IImmutableList<VarianceQuestion.Variant>;

public record VarianceQuestion(Guid Id, string QuestionText, Variants Variants, Variants Answers) : IQuestionDeclaration
{
    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor) => visitor.Visit(this);

    public record Variant(Guid Id, string AnswerText);
}

public record VarianceQuestionAnswer(Variants Variants) : IAnswer<VarianceQuestion>
{
    public T Accept<T>(IAnswerVisitor<T> visitor, VarianceQuestion question) => visitor.Visit(this, question);
}
