using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Storage.S3
{
    public interface IS3Bucket
    {
        Task<string> UploadFileAsync(string bucketName, string objectIdentification, IFormFile formFile, bool isPublic = false);
        Task<Stream> DownloadFileAsync(string bucketName, string objectIdentification);
    }
}

