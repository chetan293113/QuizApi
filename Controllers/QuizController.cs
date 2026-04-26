using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;
using QuizApi.Models;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuizController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("submit")]
    public IActionResult Submit(int userId, Dictionary<int, string> answers)
    {
        int score = 0;

        foreach (var item in answers)
        {
            var question = _context.Questions.Find(item.Key);

            if (question != null && question.CorrectAnswer == item.Value)
            {
                score++;
            }
        }

        // ✅ Save result in DB
        var result = new Result
        {
            UserId = userId,
            Score = score
        };

        _context.Results.Add(result);
        _context.SaveChanges();

        return Ok(new { Score = score });
    }
}
