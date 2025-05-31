using System.Collections.Immutable;
using System.Text.Json;
using StudentPortal.ComponentData.Quizzes;
using StudentPortal.PageView.Services;
using StudentPortal.Services.DTO;

namespace StudentPortal.PageView;

public interface ISubmissionStrategy
{
    public Task Submit(IEnumerable<QuizData> quizData);
}

public class QuizManager(IEnumerable<IQuestionDeclaration> questions, ISubmissionStrategy submissionStrategy, IAsyncKeyValueStorage<string, string> storage) : IQuizManager, IQuestionDeclarationVisitor<Task<IAnswer<IQuestionDeclaration>?>>, IAnswerVisitor<Task>
{
    public event Action OnSubmit = null!;

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
        List<QuizData> quizDataList = [];
        foreach (var item in questions)
        {
            var answer = await Get(item);

            if (answer == null) continue;

            quizDataList.Add(answer.ToQuizData());
        }

        await submissionStrategy.Submit(quizDataList);

        foreach (var item in questions)
        {
            await storage.Delete(item.Id.ToString());
        }

        OnSubmit?.Invoke();
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