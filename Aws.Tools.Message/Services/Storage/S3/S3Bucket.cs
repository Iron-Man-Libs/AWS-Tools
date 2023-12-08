using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Aws.Tools.Message.Services.Storage.S3.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Storage.S3
{
    public class S3Bucket : IS3Bucket
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3Bucket> _logger;
        private readonly S3BucketConfiguration _bucketConfiguration;

        public S3Bucket(IAmazonS3 s3Client, ILogger<S3Bucket> logger, IOptions<S3BucketConfiguration> bucketConfiguration)
        {
            _ = bucketConfiguration.Value.BaseUrl ?? bucketConfiguration.Value.S3BucketPath ?? throw new ArgumentNullException("S3BucketConfiguration is null");
            _bucketConfiguration = bucketConfiguration.Value;
            _s3Client = s3Client;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(string objectIdentification, IFormFile formFile, string bucketName, bool isPublic = false)
        {
            try
            {
                using MemoryStream newMemoryStream = new();
                formFile.CopyTo(newMemoryStream);

                string keyName = $"{bucketName}/{objectIdentification}";

                var putRequest = new PutObjectRequest
                {
                    InputStream = newMemoryStream,
                    Key = keyName,
                    BucketName = _bucketConfiguration.S3BucketPath,
                    CannedACL = isPublic ? S3CannedACL.PublicRead : S3CannedACL.Private,
                    ContentType = formFile.ContentType
                };

                await _s3Client.PutObjectAsync(putRequest);

                var region = _s3Client.Config.RegionEndpoint.SystemName;
                return $"https://{_bucketConfiguration.S3BucketPath}.s3.amazonaws.com/{keyName}";

            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "UNABLE_UPLOAD_FILE {identification}", objectIdentification);
                throw;
            }
        }

        public async Task<Stream> DownloadFileAsync(string objectIdentification)
        {
            try
            {
                TransferUtility fileTransferUtility = new(_s3Client);

                TransferUtilityOpenStreamRequest downloadRequest = new()
                {
                    BucketName = _bucketConfiguration.S3BucketPath,
                    Key = objectIdentification
                };

                return await fileTransferUtility.OpenStreamAsync(downloadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNABLE_DOWNLOAD_FILE {identification}", objectIdentification);
            }

            return null;
        }
    }
}
