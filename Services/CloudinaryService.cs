namespace CoolMate.Services
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    public class CloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            Account account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );
            cloudinary = new Cloudinary(account);
        }

        public async Task<UploadResult> UploadImageAsync(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = Path.GetFileNameWithoutExtension(file.FileName)
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }
    }

}
