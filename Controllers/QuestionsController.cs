using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;
using QuizApi.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL QUESTIONS
        [HttpGet]
        public IActionResult GetAll()
        {
            var questions = _context.Questions.ToList();
            return Ok(questions);
        }

        // ✅ GET QUESTION BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var question = _context.Questions.Find(id);

            if (question == null)
                return NotFound("Question not found");

            return Ok(question);
        }

        // ✅ ADD QUESTION
        [HttpPost]
        public IActionResult AddQuestion(Questions q)
        {
            _context.Questions.Add(q);
            _context.SaveChanges();
            return Ok(q);
        }

        // ✅ UPDATE QUESTION
        [HttpPut("{id}")]
        public IActionResult Update(int id, Questions updated)
        {
            var question = _context.Questions.Find(id);

            if (question == null)
                return NotFound("Question not found");

            question.QuestionText = updated.QuestionText;
            question.OptionA = updated.OptionA;
            question.OptionB = updated.OptionB;
            question.OptionC = updated.OptionC;
            question.OptionD = updated.OptionD;
            question.CorrectAnswer = updated.CorrectAnswer;

            _context.SaveChanges();

            return Ok(question);
        }

        // ✅ DELETE QUESTION
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var question = _context.Questions.Find(id);

            if (question == null)
                return NotFound("Question not found");

            _context.Questions.Remove(question);
            _context.SaveChanges();

            return Ok("Deleted successfully");
        }
    }
}