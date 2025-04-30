using System.Collections.Immutable;
using System.Text.Json;
using StudentPortal.ComponentData.Quizzes;
using StudentPortal.PageView.Services;

namespace StudentPortal.PageView;

public class QuizManager(IEnumerable<IQuestionDeclaration> questions, IAsyncKeyValueStorage<string, string> storage) : IQuizManager, IQuestionDeclarationVisitor<Task<IAnswer<IQuestionDeclaration>?>>, IAnswerVisitor<Task>
{
    public Task<IAnswer<IQuestionDeclaration>?> Get(IQuestionDeclaration question)
    {
        return question.Accept(this)!;
    }

    public Task Set(IAnswer<IQuestionDeclaration> answer)
    {
        return answer.Accept(this);
    }

    public async Task Submit()
    {
        Console.WriteLine("Test finished");
        Console.WriteLine("======================");
        int index = 1;
        foreach (var question in questions)
        {
            Console.WriteLine("{0}. {1}: {2}", index++, question.Accept(new QuestionToStringFormatter()), await Get(question));
        }
        Console.WriteLine("======================");
    }

    private class QuestionToStringFormatter : IQuestionDeclarationVisitor<string>
    {
        public string Visit(VarianceQuestion question)
        {
            return question.QuestionText;
        }

        public string Visit(OpenAnswerQuestion question)
        {
            return question.QuestionText;
        }
    }

    public async Task Visit(VarianceQuestionAnswer answer)
    {
        var ids = JsonSerializer.Serialize(answer.Variants.Select(x => x.Id));

        await storage.Set(answer.Question.Id.ToString(), ids);
    }

    public async Task Visit(OpenAnswerQuestionAnswer answer)
    {
        await storage.Set(answer.Question.Id.ToString(), answer.Answer);
    }

    public async Task<IAnswer<IQuestionDeclaration>?> Visit(VarianceQuestion question)
    {
        var res = await storage.Get(question.Id.ToString());

        if (!string.IsNullOrEmpty(res))
        {
            var ids = JsonSerializer.Deserialize<IEnumerable<string>>(res)!.Select(Guid.Parse);

            var variants = question.Variants.Where(v => ids.Contains(v.Id));

            return new VarianceQuestionAnswer(question, variants.ToImmutableList());
        }

        return null;
    }

    public async Task<IAnswer<IQuestionDeclaration>?> Visit(OpenAnswerQuestion question)
    {
        var res = await storage.Get(question.Id.ToString());

        if (!string.IsNullOrEmpty(res))
        {
            return new OpenAnswerQuestionAnswer(question, res);
        }

        return null;
    }
}