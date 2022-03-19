using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
using RouteSheet.Shared.Models;
using RouteSheet.Shared.ViewModels;

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
        public ActionResult<IList<Lesson>> GetLessons()
        {
            var lessons = _appRepository.AllLessons().ToList();
            return lessons;
        }
            //_appRepository.AllLessons() is IQueryable<Lesson> lessons
            //? Ok(lessons.ToList())
            //: NotFound();


        [HttpPost("add")]
        public async Task<ActionResult<Lesson>> Add(LessonAddViewModel lesson)
        {
            try
            {
                var appUser = await _appRepository.FindUserById(lesson.AppUserId);
                var cadet = await _appRepository.FindCadetById(lesson.CadetId);
                var newLesson = new Lesson
                {
                    AppUser = appUser,
                    Cadet = cadet,
                    Date = lesson.Date,
                    Hour = lesson.Hour,
                    Prioriy = lesson.Prioriy,
                    Title = lesson.Title
                };
                var createdLesson = await _appRepository.AddLesson(newLesson);
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
                var lessonToUpdate = await _appRepository.FindLessonById(lesson.Id);
                if (lessonToUpdate is null)
                    return Problem(title: "Api layer problems, see details for more info", detail: $"Can't update! Wrong id {lesson.Id}");

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
            await _appRepository.DeleteLesson(id) ? NoContent() : BadRequest();
    }
}
