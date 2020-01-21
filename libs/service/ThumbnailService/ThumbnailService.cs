using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;

namespace TutorBits.Thumbnail
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddThumbnailService(this IServiceCollection services)
        {
            return services.AddTransient<ThumbnailService>();
        }
    }

    public class ThumbnailService
    {
        private readonly FileDataLayerInterface dataLayer_;

        public readonly string ThumbnailFileName;
        public readonly string ThumbnailsDir;

        public ThumbnailService(IConfiguration configuration, FileDataLayerInterface fileDataLayer)
        {
            dataLayer_ = fileDataLayer;

            ThumbnailFileName = configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ThumbnailFileNameKey);
            ThumbnailsDir = configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ThumbnailsDirKey);
        }

        public string GetThumbnailsDirectory(string thumbnailId)
        {
            var workingDirectory = dataLayer_.GetWorkingDirectory();
            return dataLayer_.SanitizePath(string.IsNullOrWhiteSpace(workingDirectory) ?
                Path.Combine(ThumbnailsDir, thumbnailId) :
                Path.Combine(workingDirectory, ThumbnailsDir, thumbnailId));
        }

        public string GetThumbnailFilePath(string directory)
        {
            return dataLayer_.SanitizePath(Path.Combine(directory, ThumbnailFileName));
        }

        public async Task UploadThumbnail(Guid thumbnailId, Stream thumbnail)
        {
            var thumbnailsDirectoryPath = GetThumbnailsDirectory(thumbnailId.ToString());
            var thumbnailFilePath = GetThumbnailFilePath(thumbnailsDirectoryPath);

            await dataLayer_.CreateDirectory(thumbnailsDirectoryPath);
            await dataLayer_.CreateFile(thumbnailFilePath, thumbnail);
        }
    }
}

