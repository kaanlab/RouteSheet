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
    public class ClassroomsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        public ClassroomsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("all")]
        public ActionResult<IList<ClassroomViewModel>> GetClassrooms()
        {
            var classroom = _appRepository.AllClassrooms();
            var classsroomViewModels = classroom.Select(x => x.ToClassroomViewModel());

            return Ok(classsroomViewModels.ToList());
        }



        [HttpPost("add")]
        public async Task<ActionResult<ClassroomViewModel>> Add(ClassroomViewModel classroomViweModel)
        {
            try
            {
                var classroom = classroomViweModel.ToClassroomModel();
                var createdClassroom = await _appRepository.AddClassroom(classroom);
                return Ok(createdClassroom.ToClassroomViewModel());
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<ClassroomViewModel>> Update(ClassroomViewModel classroomViewModel)
        {
            try
            {
                var classroom = classroomViewModel.ToClassroomModel();
                var classroomToUpdate = await _appRepository.FindClassroomById(classroom.Id);
                if (classroomToUpdate is null)
                    return Problem(title: "Api layer problems, see details for more info", detail: $"Can't update! Wrong id {classroom.Id}");

                var updatedClassroom = await _appRepository.UpdateClassroom(classroom);
                return Ok(updatedClassroom.ToClassroomViewModel());
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
