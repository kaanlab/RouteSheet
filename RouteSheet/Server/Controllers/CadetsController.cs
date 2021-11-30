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
    public class CadetsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        public CadetsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("all")]
        public ActionResult<IList<CadetViewModel>> GetCadets()
        {
            var cadets = _appRepository.AllCadets();
            var cadetsViewModels = cadets.Select(x => x.ToCadetViewModel());
            
            return Ok(cadetsViewModels.ToList());
        }

        [HttpPost("add")]
        public async Task<ActionResult<CadetViewModel>> Add(CadetViewModel cadetViewModel)
        {
            try
            {
                var cadet = cadetViewModel.ToCadetModel();
                var createdCadet = await _appRepository.AddCadet(cadet);
                return Ok(createdCadet.ToCadetViewModel());
            }
            catch (AppRepositoryException ex)
            {
                return Problem(title: ex.Message, detail: ex.InnerException.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<CadetViewModel>> Update(CadetViewModel cadetViewModel)
        {
            try
            {
                var cadet = cadetViewModel.ToCadetModel();
                var cadetToUpdate = await _appRepository.FindCadetById(cadet.Id);
                if (cadetToUpdate is null)
                    return Problem(title: "Api layer problems, see details for more info", detail: $"Can't update! Wrong id {cadet.Id}");

                var updatedCadet = await _appRepository.UpdateCadet(cadet);
                return Ok(updatedCadet.ToCadetViewModel());
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
