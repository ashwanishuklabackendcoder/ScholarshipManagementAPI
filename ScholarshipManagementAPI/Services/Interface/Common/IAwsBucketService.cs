namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface IAwsBucketService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteFileAsync(string fileKey);
        string GeneratePreSignedUrl(string fileKey);
    }
}
