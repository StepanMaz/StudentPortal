using Microsoft.EntityFrameworkCore;

namespace StudentPortal.QuizService.DB.Entities;

#nullable disable
[Owned]
public class QuizResult
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public string QuestionData { get; set; }
    public string Answer { get; set; }
    public string AnswerData { get; set; }
    public double MaxScore { get; set; }
    public double? Score { get; set; }
}
#nullable restore