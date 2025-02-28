using System.Collections.Immutable;

namespace StudentPortal.ComponentData.Quizzes;

using Variants = IImmutableList<MultiAnswerQuestion.Variant>;

public record MultiAnswerQuestion(Guid Id, string QuestionText, Variants Variants, Variants Answers) : IQuestionDeclaration
{
    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor) => visitor.Visit(this);

    public record Variant(Guid Id, string AnswerText);
}

public record MultiAnswerQuestionAnswer(Variants Variants) : IAnswer<MultiAnswerQuestion>
{
    public T Accept<T>(IAnswerVisitor<T> visitor, MultiAnswerQuestion question) => visitor.Visit(this, question);
}
