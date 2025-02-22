﻿using Domain.DTOs.Auth;
using Domain.DTOs.Course;
using Domain.DTOs.Lesson;
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

        [HttpGet("public/{id}")]
        public async Task<IActionResult> GetInfoPublic(int id)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var course = await _service.GetInfoPublic(id, userId is null ? 0 : int.Parse(userId));
                if(course is null)
                {
                    return NotFound(new Response<string>(false, "Curso no encontrado"));
                }

                if (userId is not null)
                {
                    var hasCourse = await _service.HasPurchasedCourse(id, int.Parse(userId));
                    course.Purchased = hasCourse;
                }


                return Ok(new Response<CoursePublicOutDTO>(true, "Curso encontrado exitosamente", course));
                
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult> Create([FromBody] CourseTitleInDTO dataCourse)
        {
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

                if (String.IsNullOrEmpty(dataCourse.Title.Trim()))
                {
                    return BadRequest(new Response<string>(false, "El título no puede estar vacío"));
                }

                var newId = await _service.CreateCourse(dataCourse, userIdInt);

                return Ok(new Response<int>(true, "Curso creado de manera exitosa", newId));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
        
        [Authorize]
        [HttpGet("instructor")]
        public async Task<ActionResult> GetCoursesByUserId()
        {
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

                var courses = await _service.GetCoursesByUserId(userIdInt);

                return Ok(new Response<IEnumerable<CourseMineOutDTO>>(true, "Cursos obtenidos de manera exitosa", courses));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
        
        [Authorize]
        [HttpGet("lesson/{id}")]

        public async Task<ActionResult> GetLesson(int id)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var isInstructor = User.FindFirst("IsInstructor")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                var lesson = await _service.GetLesson(id);

                if (lesson is null)
                {
                    return NotFound(new Response<string>(false, "Lección no encontrada"));
                }

                return Ok (new Response<LessonOutDTO>(true, "Lección obtenida de manera exitosa", lesson));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
        
        [Authorize]
        [HttpDelete("{courseId}")]
        public async Task<ActionResult> DeleteCourse(int courseId)
        {
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

                await _service.DeleteCourse(courseId, userIdInt);

                return Ok(new Response<string?>(true, "Cursos eliminado de manera exitosa", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [Authorize]
        [HttpPut("publish/{courseId}")]
        public async Task<ActionResult> PublishCourse(int courseId)
        {
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

                await _service.PublishCourse(courseId, userIdInt);

                return Ok(new Response<string?>(true, "Cursos publicado de manera exitosa", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [HttpGet("popular")]
        public async Task<ActionResult> GetPopularCourses([FromQuery] int limit = 5)
        {
            try
            {
                if (limit <= 0)
                {
                    limit = 5;
                }

                var courses = await _service.GetPopularCourses(limit);

                return Ok(new Response<IEnumerable<CourseSearchDTO>>(true, "Cursos obtenidos de manera exitosa", courses));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [Authorize]
        [HttpGet("purchased")]
        public async Task<ActionResult> GetPurchasedCourses()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                var courses = await _service.GetPurchasedCourses(userIdInt);

                return Ok(new Response<IEnumerable<CourseSearchDTO>>(true, "Cursos obtenidos de manera exitosa", courses));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
        [Authorize]
        [HttpGet("in-process")]
        public async Task<ActionResult> GetInProcessCourses()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                var courses = await _service.GetInProcessCourses(userIdInt);

                return Ok(new Response<IEnumerable<CourseSearchDTO>>(true, "Cursos obtenidos de manera exitosa", courses));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
        [Authorize]
        [HttpGet("done")]
        public async Task<ActionResult> GetDoneCourses()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                var courses = await _service.GetDoneCourses(userIdInt);

                return Ok(new Response<IEnumerable<CourseSearchDTO>>(true, "Cursos obtenidos de manera exitosa", courses));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
    }
}
