using Microsoft.AspNetCore.Mvc;
using educat_api.Services;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Domain.DTOs.Instructor;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly InstructorService _service;
        public InstructorController(InstructorService service)
        {
            _service = service;
        }
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<InstructorInfoDTO>> GetInstructor()
        {
            var id = User.FindFirst("ID")?.Value;
            if (id == null)
            {
                return BadRequest(new Response<string>(false, "No se encontró el ID del usuario", ""));
            }
            try
            {
                var UserId = int.Parse(id);
                
                var instructor = await _service.GetInstructorById(UserId);
                if (instructor == null)
                {
                    return NotFound(new Response<string>(false, "No se encontró el instructor", ""));
                }
                return Ok(new Response<InstructorInfoDTO>(true, "Instructor encontrado", instructor));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpPut("edit")]
        [Authorize]
        public async Task<ActionResult<InstructorInfoDTO>> UpdateInstructor(InstructorInfoDTO request)
        {
            var id = User.FindFirst("ID")?.Value;
            if (id == null)
            {
                return BadRequest(new Response<string>(false, "No se encontró el ID del usuario", ""));
            }
            try
            {
                var instructor = await _service.updateInstructor(request);
                return Ok(new Response<InstructorInfoDTO>(true, "Instructor actualizado", instructor));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}