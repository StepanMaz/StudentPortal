namespace StudentPortalServer.Quizzes;

using AnswerDataCollection = IEnumerable<TestQuestion.AnswerData>;

public record TestQuestion : IQuestionDeclaration
{
    public required QuestionIdentifier Id { get; init; }

    public required string QuestionText { get; init; }

    public required AnswerDataCollection Variants { get; init; }
    public required AnswerDataCollection Answers { get; init; }

    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor) => visitor.Visit(this);

    public record AnswerData(AnswerIdentifier Id, string Text);
}


public record TestQuestionAnswer(TestQuestion Question, AnswerDataCollection Answers) : IAnswer<TestQuestion>
{
    public T Accept<T>(IAnswerVisitor<T> visitor) => visitor.Visit(this);
}

public partial class AsyncKeyValueStorageAdapter
{
    public async Task<IAnswer?> Visit(TestQuestion question)
    {
        var answerIds = await storage.GetAsync<string[]>(question.Id);
        
        if (answerIds is null) return null;

        var parsedIds = answerIds.Select(AnswerIdentifier.Parse);

        var answers = question.Variants.Where(x => parsedIds.Contains(x.Id)).ToArray();

        return new TestQuestionAnswer(question, answers);
    }

    public async Task Visit(TestQuestionAnswer answer)
    {
        await storage.SetAsync(answer.Question.Id, answer.Answers.Select(x => x.Id.ToString()).ToArray());
    }
}