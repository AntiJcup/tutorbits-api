using System;
using Microsoft.Extensions.Configuration;

namespace Utils.Common
{
    //Generates urls for project related resources
    public static class ProjectUrlGenerator
    {
        public static string GenerateTransactionLogUrl(string transactionLogFileName, Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectTransactionPathKey);

            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString(), transactionLogFileName));
        }

        public static string GenerateProjectUrl(Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectPathKey);
            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString()));
        }

        public static string GenerateProjectVideoUrl(Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectVideoPathKey);
            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString()));
        }

        public static string GenerateProjectPreviewUrl(string previewId, Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectPreviewPathKey);
            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString(), previewId));
        }

        public static string GenerateProjectThumbnailUrl(Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectThumbnailPathKey);
            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString()));
        }

        public static string GenerateProjectDownloadUrl(Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectZipPathKey);
            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString()));
        }

        public static string GenerateProjectJsonUrl(Guid projectId, IConfiguration configuration)
        {
            var host = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectHostKey);
            var path = configuration.GetSection(Constants.Configuration.Sections.UrlsKey)
                                                .GetValue<string>(Constants.Configuration.Sections.Urls.ProjectJsonPathKey);
            return string.Format("{0}/{1}", host, string.Format(path, projectId.ToString()));
        }
    }
}
