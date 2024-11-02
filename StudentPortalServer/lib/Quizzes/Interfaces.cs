namespace StudentPortalServer.Quizzes;

#region Data interfaces

public interface IAnswer
{
    public K Accept<K>(IAnswerVisitor<K> visitor);
}

public interface IAnswer<T> : IAnswer
{
    public T Question { get; }
}

public interface IQuestionDeclaration
{
    public T Accept<T>(IQuestionDeclarationVisitor<T> visitor);
}

#endregion


#region Visitors

public interface IAnswerVisitor<T>
{
    public T Visit(TestQuestionAnswer answer);
}

public interface IQuestionDeclarationVisitor<T>
{
    public T Visit(TestQuestion question);
}

#endregion