using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lawer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _storagePath;


        public FileController(IConfiguration configuration)
        {
            _storagePath = configuration["FileUpload:StoragePath"];
        }
        [HttpGet("GetFile")]
        public IActionResult Downloadfile(string fileName)
        {
            var fullPath = Path.Combine(_storagePath, fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("file not found");
            }
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var contenttype = "application/octet-stream";

            return PhysicalFile(fullPath, contenttype, fileName);
        }

    }
}
