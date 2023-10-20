using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Storage.S3
{
    public class S3Bucket : IS3Bucket
    {
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 _s3Client;
        private readonly ILogger<S3Bucket> _logger;
        private readonly TransferUtility _fileTransferUtility;

        public S3Bucket(ILogger<S3Bucket> logger)
        {
            _s3Client = new AmazonS3Client(bucketRegion);
            _logger = logger;
            _fileTransferUtility = new(_s3Client);
        }

        public async Task<string> UploadFileAsync(string bucketName, string objectIdentification, IFormFile formFile, bool isPublic = false)
        {
            try
            {
                using MemoryStream newMemoryStream = new();
                formFile.CopyTo(newMemoryStream);

                var fileName = $"{objectIdentification}{Path.GetExtension(formFile.FileName)}";

                TransferUtilityUploadRequest uploadRequest = new()
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    BucketName = bucketName,
                    CannedACL = isPublic ? S3CannedACL.PublicRead : S3CannedACL.Private
                };

                await _fileTransferUtility.UploadAsync(uploadRequest);

                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNABLE_UPLOAD_FILE {identification}", objectIdentification);
            }

            return null;
        }

        public async Task<Stream> DownloadFileAsync(string bucketName, string objectIdentification)
        {
            try
            {
                TransferUtility fileTransferUtility = new(_s3Client);

                TransferUtilityOpenStreamRequest downloadRequest = new()
                {
                    BucketName = bucketName,
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
