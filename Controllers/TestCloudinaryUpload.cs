using CoolMate.Services;
using Microsoft.AspNetCore.Mvc;


namespace CoolMate.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class TestCloudinaryUpload : ControllerBase
    {
        private readonly CloudinaryService cloudinaryService;

        public TestCloudinaryUpload(CloudinaryService cloudinaryService)
        {
            this.cloudinaryService = cloudinaryService;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            var uploadResult = await cloudinaryService.UploadImageAsync(file);

            
            return Ok(uploadResult.Url);
        }
    }
}
