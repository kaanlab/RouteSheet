using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
using RouteSheet.Shared.Models;

namespace RouteSheet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadetsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        public CadetsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("all")]
        public ActionResult<IList<Cadet>> GetCadets() =>
            _appRepository.AllCadets() is IQueryable<Cadet> cadets
            ? Ok(cadets.ToList())
            : NotFound();


        [HttpPost("add")]
        public async Task<ActionResult<Cadet>> Add(Cadet cadet)
        {
            try
            {
                var createdCadet = await _appRepository.AddCadet(cadet);
                return Ok(createdCadet);
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<Cadet>> Update(Cadet cadet)
        {
            try
            {
                var cadetToUpdate = await _appRepository.FindCadetById(cadet.Id);
                if (cadetToUpdate is null)
                    return Problem(title: "Api layer problems, see details for more info", detail: $"Can't update! Wrong id {cadet.Id}");

                var updatedCadet = await _appRepository.UpdateCadet(cadet);
                return Ok(updatedCadet);
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id) =>
            await _appRepository.DeleteCadet(id) ? NoContent() : BadRequest();
    }
}
