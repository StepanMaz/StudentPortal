using StudentPortalServer.Services;

namespace StudentPortalServer.Quizzes;

public interface IQuizStorage {
    Task<IAnswer?> Get(IQuestionDeclaration question);
    Task Set(IAnswer answer); 
}

public partial class AsyncKeyValueStorageAdapter(IAsyncKeyValueStorage storage) : IQuizStorage, IQuestionDeclarationVisitor<Task<IAnswer?>>, IAnswerVisitor<Task>
{
    public Task<IAnswer?> Get(IQuestionDeclaration question)
    {
        return question.Accept(this);
    }

    public Task Set(IAnswer answer)
    {
        return answer.Accept(this);
    }
}