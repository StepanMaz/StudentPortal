namespace StudentPortalServer.Quizzes;

public record QuestionIdentifier(Guid Guid)
{
    public static QuestionIdentifier New()
    {
        return new QuestionIdentifier(Guid.NewGuid());
    }

    public static QuestionIdentifier Parse(string data)
    {
        return new QuestionIdentifier(Guid.Parse(data));
    }
    
    public static implicit operator string(QuestionIdentifier wrapper) => wrapper.ToString();

    public override string ToString() => Guid.ToString();
}

public record AnswerIdentifier(Guid Guid)
{
    public static AnswerIdentifier New()
    {
        return new AnswerIdentifier(Guid.NewGuid());
    }

    public static AnswerIdentifier Parse(string data)
    {
        return new AnswerIdentifier(Guid.Parse(data));
    }

    
    public static implicit operator string(AnswerIdentifier wrapper) => wrapper.ToString();

    public override string ToString() => Guid.ToString();
}