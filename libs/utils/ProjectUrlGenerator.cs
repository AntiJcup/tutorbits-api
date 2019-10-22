﻿using System;
using Microsoft.Extensions.Configuration;

namespace Utils
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
    }
}
