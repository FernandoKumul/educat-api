using Domain.DTOs.Category;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;
        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<CategoryOutDTO>>>> GetAllCategories()
        {
            try
            {
                var categories = await _service.GetAll();
                return Ok(new Response<IEnumerable<CategoryOutDTO>>(true, "Categorías obtenidas exitosamente", categories));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, "Ocurrió un error: " + ex.Message));
            }
        }
    }
}
