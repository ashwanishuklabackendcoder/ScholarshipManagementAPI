using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ScholarshipManagementAPI.DTOs.Common.Settings;
using ScholarshipManagementAPI.Services.Interface.Common;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class AwsBucketService : IAwsBucketService
    {
        private readonly AwsS3OptionsDto _options;
        private readonly IAmazonS3 _s3Client;

        public AwsBucketService(
            IOptions<AwsS3OptionsDto> options,
            IAmazonS3 s3Client)
        {
            _options = options.Value;
            _s3Client = s3Client;
        }


        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,

                //CannedACL = S3CannedACL.Private
                //If you want uploaded files accessible with URL later,
            };

            await _s3Client.PutObjectAsync(request);

            return fileName;
        }

        public async Task DeleteFileAsync(string fileKey)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fileKey
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        public string GeneratePreSignedUrl(string fileKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _options.BucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            return _s3Client.GetPreSignedURL(request);
        }

    }
}
