using Domain.DTOs.Course;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseSearchController : ControllerBase
    {
        private readonly CourseSearchService _service;
        public CourseSearchController(CourseSearchService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchCourses([FromQuery] PaginationParameters parameters)
        {
            var PageNumber = parameters.PageNumber;
            var PageSize = parameters.PageSize;
            try
            {
                (object result, int count) = await _service
                    .Search(PageNumber, PageSize, parameters.Search);

                return Ok(new Response<object>(true, "Datos obtenidos exitosamente", new { result, count }));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, "Ocurrió un error: " + ex.Message));
            }
        }
    }
}