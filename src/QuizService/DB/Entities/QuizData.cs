namespace StudentPortal.QuizService.DB.Entities;

#nullable disable
public class QuizData
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<QuizResult> Results { get; set; }
}
#nullable restore