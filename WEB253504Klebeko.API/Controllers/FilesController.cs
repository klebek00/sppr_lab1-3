using Microsoft.AspNetCore.Mvc;

namespace WEB253504Klebeko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }
            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);
            // если такой файл существует, удалить его
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            // скопировать файл в поток
            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);
            // получить Url файла
            var host = HttpContext.Request.Host;
            var fileUrl = $"Https://{host}/Images/{file.FileName}";
            return Ok(fileUrl);
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);

            // Проверяем, существует ли файл
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = $"Файл {fileName} не найден." });
            }

            try
            {
                System.IO.File.Delete(filePath);
                return Ok(new { message = $"Файл {fileName} был успешно удален." });
            }
            catch (Exception ex)
            {
                // Возвращаем ошибку в случае проблем с удалением
                return StatusCode(500, new { message = $"Ошибка при удалении файла: {ex.Message}" });
            }
        }



    }
}
