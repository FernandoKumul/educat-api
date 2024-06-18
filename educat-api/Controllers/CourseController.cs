using Domain.DTOs.Auth;
using Domain.DTOs.Course;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("to-edit/{id}")]
        public async Task<IActionResult> GetToEdit(int id)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var isInstructor = User.FindFirst("IsInstructor")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                if (!Boolean.TryParse(isInstructor, out bool isInstructorBool))
                {
                    return Forbid("Token no valido");
                }

                if(!isInstructorBool)
                {
                    return new ObjectResult(new Response<string>(false, "No eres un instructor")) { StatusCode = 403 };
                }

                var course = await _service.GetToEdit(id, userIdInt);

                if(course == null)
                {
                    return NotFound(new Response<string>(false, "Curso no encontrado"));
                }

                return Ok(new Response<CourseEditOutDTO>(true, "Curso obtenido", course));
            }
            catch (Exception ex)
            {

                //Cambiar por un error 500
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [Authorize]
        [HttpPut("save-draft/{id}")]
        public async Task<ActionResult<Response<string>>> Update(int id, [FromBody] CourseSaveInDTO updateCourse)
        {

            //for (int i = 0; i < updateTest.Questions.Count; i++)
            //{
            //    int nCorrect = 0;
            //    foreach (var answer in updateTest.Questions[i].Answers)
            //    {
            //        if (answer.Correct) nCorrect++;
            //    }

            //    if (nCorrect == 0)
            //    {
            //        return BadRequest(new Response<string>(false, $"La pregunta[{i}] no tiene ninguna respuesta correcta"));
            //    }
            //}

            //string[] visibilityTypes = { "unlisted", "private", "public" };
            //if (!Array.Exists(visibilityTypes, color => color == updateTest.Visibility))
            //{
            //    return BadRequest(new Response<int>(false, "No está ingresando algún tipo de visibilidad valido"));
            //}

            //string[] colors = { "green", "blue", "purple", "orange", "yellow", "red" };

            //if (!Array.Exists(colors, color => color == updateTest.Color))
            //{
            //    return BadRequest(new Response<int>(false, "No está ingresando algún color valido"));
            //}

            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var isInstructor = User.FindFirst("IsInstructor")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                if (!Boolean.TryParse(isInstructor, out bool isInstructorBool))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };
                }

                if (!isInstructorBool)
                {
                    return new ObjectResult(new Response<string>(false, "No eres un instructor")) { StatusCode = 403 };
                }

                await _service.SaveCourse(updateCourse, id, userIdInt);

                return Ok(new Response<string>(true, "Curso guardado de manera exitosa"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

    }
}
