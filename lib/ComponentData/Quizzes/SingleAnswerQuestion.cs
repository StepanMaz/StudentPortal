using System.Collections.Immutable;

namespace StudentPortal.ComponentData.Quizzes;

using Variants = IImmutableList<SingleAnswerQuestion.Variant>;

public record SingleAnswerQuestion(Guid Id, string QuestionText, Variants Variants, SingleAnswerQuestion.Variant Answer) : IQuestionDeclaration
{
    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor) => visitor.Visit(this);

    public record Variant(Guid Id, string AnswerText);
}

public record SingleAnswerQuestionAnswer(SingleAnswerQuestion.Variant Variant) : IAnswer<SingleAnswerQuestion>
{
    public T Accept<T>(IAnswerVisitor<T> visitor, SingleAnswerQuestion question) => visitor.Visit(this, question);
}
