using MudBlazor;
using StudentPortal.ComponentData.Quizzes;

namespace StudentPortal.PageEditor;

public class EditorQuizManager(ISnackbar snackbar) : IQuizManager
{
    private Dictionary<IQuestionDeclaration, IAnswer<IQuestionDeclaration>> _answers = new();

    public Task<IAnswer<IQuestionDeclaration>?> Get(IQuestionDeclaration question)
    {
        return Task.FromResult(_answers[question])!;
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
