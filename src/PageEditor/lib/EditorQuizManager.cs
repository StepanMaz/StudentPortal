using MudBlazor;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.PageEditor;

public class EditorQuizManager(ISnackbar snackbar) : IQuizManager
{
    private Dictionary<IQuestionDeclaration, IAnswer<IQuestionDeclaration>> _answers = new();

    public Task<IAnswer<IQuestionDeclaration>?> Get(IQuestionDeclaration question)
    {
        if (_answers.TryGetValue(question, out var answer))
        {
            return Task.FromResult(answer)!;
        }

        return Task.FromResult<IAnswer<IQuestionDeclaration>?>(null);
    }

    public Task Set(IAnswer<IQuestionDeclaration> answer)
    {
        _answers[answer.Question] = answer;

        return Task.CompletedTask;
    }

    public Task Submit()
    {
        snackbar.Add("Submitting quiz in editor does nothing.", Severity.Info);

        return Task.CompletedTask;
    }
}
