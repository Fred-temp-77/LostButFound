using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace LostButFound.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var cloudName = Environment.GetEnvironmentVariable("CloudinarySettings__CloudName")
                            ?? config["CloudinarySettings:CloudName"];

            var apiKey = Environment.GetEnvironmentVariable("CloudinarySettings__ApiKey")
                         ?? config["CloudinarySettings:ApiKey"];

            var apiSecret = Environment.GetEnvironmentVariable("CloudinarySettings__ApiSecret")
                            ?? config["CloudinarySettings:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }
    }
}
