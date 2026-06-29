namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface ILocalFileService
    {
        Task<string> UploadAsync(Stream stream, string fileKey);
        string GetFileUrl(string fileKey);
    }
}
