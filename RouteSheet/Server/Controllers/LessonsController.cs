using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IResult GetLessons() =>
            _appRepository.GetLessons() is IQueryable<Lesson> lessons
            ? Results.Ok(lessons)
            : Results.NotFound();
    
    }
    
}
