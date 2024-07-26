using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Domain.Utilities;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        public FileController(IConfiguration configuration)
        {
            var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        [HttpPost("image")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new Response<string>(false, "Ningún archivo subido"));
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return BadRequest(new Response<string>(false, $"Error al subir la imagen: {uploadResult.Error.Message}"));
                }

                return Ok(new Response<object>(true, "Imagen subida exitosamente", new { url = uploadResult.SecureUrl }));
            }
        }

        [HttpPost("video")]
        public async Task<IActionResult> UploadVideo(IFormFile video)
        {
            if (video == null || video.Length == 0)
            {
                return BadRequest(new Response<string>(false, "Ningún archivo subido"));
            }

            using (var stream = video.OpenReadStream())
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(video.FileName, stream),
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return BadRequest(new Response<string>(false, $"Error al subir el video: {uploadResult.Error.Message}"));
                }

                return Ok(new Response<object>(true, "Video subida exitosamente", new { url = uploadResult.Url, duration = uploadResult.Duration, format = uploadResult.Format }));
            }
        }
    }
}
