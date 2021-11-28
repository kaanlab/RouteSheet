using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
using RouteSheet.Shared.Models;

namespace RouteSheet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        public ClassroomsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("all")]
        public ActionResult<IList<Classroom>> GetClassrooms() =>
            _appRepository.AllClassrooms() is IQueryable<Classroom> classrooms
            ? Ok(classrooms.ToList())
            : NotFound();


        [HttpPost("add")]
        public async Task<ActionResult<Classroom>> Add(Classroom classroom)
        {
            try
            {
                var createdClassroom = await _appRepository.AddClassroom(classroom);
                return Ok(createdClassroom);
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<Classroom>> Update(Classroom classroom)
        {
            try
            {
                var classroomToUpdate = await _appRepository.FindClassroomById(classroom.Id);
                if (classroomToUpdate is null)
                    return Problem(title: "Api layer problems, see details for more info", detail: $"Can't update! Wrong id {classroom.Id}");

                var updatedClassroom = await _appRepository.UpdateClassroom(classroom);
                return Ok(updatedClassroom);
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id) =>
            await _appRepository.DeleteClassroom(id) ? NoContent() : BadRequest();
    }
}
