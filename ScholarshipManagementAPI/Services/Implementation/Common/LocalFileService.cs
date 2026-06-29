using ScholarshipManagementAPI.Services.Interface.Common;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class LocalFileService : ILocalFileService
    {
        private readonly string _basePath;
        private readonly IConfiguration _configuration;

        public LocalFileService(IConfiguration configuration)
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            _configuration = configuration;
        }

        public async Task<string> UploadAsync(Stream stream, string fileKey)
        {
            var fullPath = Path.Combine(_basePath, fileKey);

            var directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            return fileKey;
        }

        public string GetFileUrl(string fileKey)
        {
            //if (string.IsNullOrEmpty(fileKey))
            //    return null;

            var baseUrl = _configuration["AppSettings:BackendUrl"]?.TrimEnd('/');  // from appsettings
            return $"{baseUrl}/uploads/{fileKey}";
        }
    }
}
