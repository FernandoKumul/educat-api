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
    }
}