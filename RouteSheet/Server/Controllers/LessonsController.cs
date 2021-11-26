using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
using RouteSheet.Shared.Models;

namespace RouteSheet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        public LessonsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("all")]
        public ActionResult<IList<Lesson>> GetLessons() =>
            _appRepository.AllLessons() is IQueryable<Lesson> lessons
            ? Ok(lessons.ToList())
            : NotFound();


        [HttpPost("add")]
        public async Task<ActionResult<Lesson>> Add(Lesson lesson)
        {
            try
            {
                var createdLesson = await _appRepository.AddLesson(lesson);
                return Ok(createdLesson);
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<Lesson>> Update(Lesson lesson)
        {
            try
            {
                var updatedLesson = await _appRepository.UpdateLesson(lesson);
                return Ok(updatedLesson);
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id) => 
            await _appRepository.DeleteLesson(id) is 1 ? NoContent() : BadRequest();
    }
}
