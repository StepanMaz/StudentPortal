using System.Net.Http.Json;
using System.Text.Json;
using StudentPortal.ComponentData.Quizzes;


namespace StudentPortal.Services.DTO
{
    public record QuizData(QuestionData Question, AnswerData Answer, ScoreData Score);
    public record QuestionData(string Text, string Type, string Data);
    public record AnswerData(string Text, string Data);
    public record ScoreData(double? Score, double MaxScore);

    public record SubmissionData(Guid QuizId, Guid UserId, IEnumerable<QuizData> Data);
    public record Quiz(Guid Id, Guid QuizId, Guid UserId, IEnumerable<QuizData> Data);

    public static class QuizDataExtensions
    {
        public static QuizData ToQuizData(this IAnswer<IQuestionDeclaration> answer)
        {
            return answer.Accept(new QuizDataConverter());
        }

        private class QuizDataConverter : IAnswerVisitor<QuizData>
        {
            public QuizData Visit(VarianceQuestionAnswer answer)
            {
                return new(
                    Question: new(answer.Question.QuestionText, "VarianceQuestion", answer.Question.Id.ToString()),
                    Answer: new(
                        string.Join(", ", answer.Variants.Select(x => x.AnswerText)),
                        JsonSerializer.Serialize(answer.Variants.Select(x => x.Id).ToArray())
                    ),
                    Score: new(answer.GetScore(), answer.Question.MaxScore)
                );
            }

            public QuizData Visit(OpenAnswerQuestionAnswer answer)
            {
                return new(new(answer.Question.QuestionText, "OpenQuestion", answer.Question.Id.ToString()), new(answer.Answer, ""), new(answer.GetScore(), answer.Question.MaxScore));
            }
        }
    }
}

namespace StudentPortal.Services
{
    using StudentPortal.Services.DTO;

    public interface IQuizService
    {
        Task<Quiz> Publish(SubmissionData quiz);
    }

    public class QuizService(HttpClient httpClient) : IQuizService
    {
        public async Task<Quiz> Publish(SubmissionData quiz)
        {
            var res = await httpClient.PutAsJsonAsync("quiz", quiz);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to publish quiz result. \nStatus Code: {res.StatusCode}\nContent: {await res.Content.ReadAsStringAsync()}");
            }

            var content = await res.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<Quiz>(content)!;
            }
            catch
            {
                throw new Exception($"Failed to parse Quiz. Content: {content}");
            }
        }
    }
}