using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;

using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;

namespace TutorBits
{
    namespace S3FileSystem
    {
        public static class ServiceExtensions
        {
            public static IServiceCollection AddS3FileDataAccessLayer(this IServiceCollection services)
            {
                services.AddTransient<FileDataLayerInterface, S3FileDataLayerInterface>();
                return services.AddTransient<FileDataAccessService>();
            }
        }

        public class S3FileDataLayerInterface : FileDataLayerInterface
        {
            public readonly string BucketName;

            private readonly IAmazonS3 s3Client_;

            private readonly IConfiguration configuration_;

            private readonly int MaxDelete = 1000;

            public S3FileDataLayerInterface(IConfiguration config, IAmazonS3 s3Client)
            {
                configuration_ = config;
                s3Client_ = s3Client;

                BucketName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.BucketKey);
            }

            public static string RootPath(string path) //TODO REMOVE
            {
                return Path.IsPathRooted(path) ? path : Path.Combine("", path);
            }

            public string GetWorkingDirectory() //TODO REMOVE
            {
                return "";
            }

            public async Task CreateDirectory(string path)
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = BucketName;
                putObjectRequest.Key = path.EndsWith("/") ? path : (path + "/");

                await s3Client_.PutObjectAsync(putObjectRequest);
            }

            public async Task CreateFile(string path, Stream stream)
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = BucketName;
                putObjectRequest.Key = path;
                putObjectRequest.InputStream = stream;

                await s3Client_.PutObjectAsync(putObjectRequest);
            }

            public async Task DeleteDirectory(string path)
            {
                var files = await GetAllFiles(path);

                var total = 0;
                while (total < files.Count)
                {
                    var count = 0;
                    var deleteObjectsRequest = new DeleteObjectsRequest();
                    deleteObjectsRequest.BucketName = BucketName;
                    foreach (var file in files.Skip(total))
                    {
                        deleteObjectsRequest.AddKey(file);
                        ++total;
                        if (++count == MaxDelete)
                        {
                            break;
                        }
                    }
                    await s3Client_.DeleteObjectsAsync(deleteObjectsRequest);
                }

                var deleteObjectRequest = new DeleteObjectRequest();
                deleteObjectRequest.BucketName = BucketName;
                deleteObjectRequest.Key = path.EndsWith("/") ? path : (path + "/");

                await s3Client_.DeleteObjectAsync(deleteObjectRequest);
            }

            public async Task DeleteFile(string path)
            {
                var deleteObjectRequest = new DeleteObjectRequest();
                deleteObjectRequest.BucketName = BucketName;
                deleteObjectRequest.Key = path;

                await s3Client_.DeleteObjectAsync(deleteObjectRequest);
            }

            public async Task<bool> DirectoryExists(string path)
            {
                var request = new ListObjectsRequest
                {
                    BucketName = BucketName,
                    Prefix = path,
                    MaxKeys = 1
                };

                var response = await s3Client_.ListObjectsAsync(request, CancellationToken.None);

                return response.S3Objects.Any();
            }

            public async Task<bool> FileExists(string path)
            {
                var request = new ListObjectsRequest
                {
                    BucketName = BucketName,
                    Prefix = path,
                    MaxKeys = 1
                };

                var response = await s3Client_.ListObjectsAsync(request);

                return response.S3Objects.Any();
            }

            public async Task<ICollection<string>> GetAllFiles(string parentPath)
            {
                var request = new ListObjectsRequest
                {
                    BucketName = BucketName,
                    Prefix = parentPath,
                };

                var response = await s3Client_.ListObjectsAsync(request);

                return response.S3Objects.Select(o => o.Key).ToArray();
            }

            public async Task<Stream> ReadFile(string path)
            {
                var request = new GetObjectRequest()
                {
                    BucketName = BucketName,
                    Key = path,
                };

                var response = await s3Client_.GetObjectAsync(request);
                using (var fileStream = response.ResponseStream)
                {
                    var outMemoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(outMemoryStream);
                    outMemoryStream.Position = 0;
                    return outMemoryStream;
                }
            }

            public async Task<string> StartMultipartUpload(string path)
            {
                var multipartUploadStartRequest = new InitiateMultipartUploadRequest()
                {
                    BucketName = BucketName,
                    Key = path
                };

                var response = await s3Client_.InitiateMultipartUploadAsync(multipartUploadStartRequest);
                return response.UploadId;
            }

            public async Task UploadPart(string path, string multipartUploadId, int part, Stream stream)
            {
                var multipartUploadStartRequest = new UploadPartRequest()
                {
                    BucketName = BucketName,
                    Key = path,
                    UploadId = multipartUploadId
                };

                await s3Client_.UploadPartAsync(multipartUploadStartRequest);
            }

            public async Task<string> StopMultipartUpload(string path, string multipartUploadId, string destinationPath)
            {
                var multipartUploadStartRequest = new CompleteMultipartUploadRequest()
                {
                    BucketName = BucketName,
                    Key = path,
                    UploadId = multipartUploadId
                };

                await s3Client_.CompleteMultipartUploadAsync(multipartUploadStartRequest);
                return destinationPath;
            }

            public async Task UpdateFile(string path, Stream stream, bool append = false)
            {
                path = RootPath(path);
                if (!append)
                {
                    await CreateFile(path, stream);
                    return;
                }

                using (var writeStream = File.OpenWrite(path))
                {
                    writeStream.Seek(0, SeekOrigin.End);
                    await stream.CopyToAsync(writeStream);
                }
            }

            public async Task CreatePathForFile(string filePath)
            {
                //Not necessary for s3
            }

            public async Task<string> ConvertToNativePath(string path)
            {
                return path.Replace('/', '\\');
            }
        }
    }
}