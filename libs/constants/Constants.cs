using System;

public static class Constants
{
    public static class Configuration
    {
        public static class Sections
        {
            public static readonly string SettingsKey = "Settings";

            public static readonly string PathsKey = "Paths";

            public static readonly string UrlsKey = "UrlSettings";

            public static class Settings
            {
                public static readonly string PartitionSizeKey = "PartitionSize";

                public static readonly string UseAWSKey = "UseAWS";

                public static readonly string TranscoderPresetIdKey = "TranscoderPresetId";

                public static readonly string TranscoderPipelineIdKey = "TranscoderPipelineId";

                public static readonly string UserPoolClientIdKey = "UserPoolClientId";

                public static readonly string UserPoolIdKey = "UserPoolId";

                public static readonly string UserPoolAuthorityKey = "UserPoolAuthority";

                public static readonly string LocalAdminKey = "LocalAdmin";

                public static readonly string FinalizeProjectLambdaNameKey = "FinalizeProjectLambdaName";

                public static readonly string GoogleExternalGroupNameKey = "GoogleExternalGroupName";

                public static readonly string NormalizeVideoLambdaNameKey = "NormalizeVideoLambdaName";

                public static readonly string HealthCheckLambdaNameKey = "HealthCheckLambdaName";

                public static readonly string TranscodeTimeoutSecondsKey = "TranscodeTimeoutSeconds";
            }

            public static class Paths
            {
                public static readonly string BucketKey = "Bucket";

                public static readonly string ProjectsDirKey = "ProjectsDir";

                public static readonly string ProjectFileNameKey = "ProjectFileName";

                public static readonly string TransactionsDirKey = "TransactionsDir";

                public static readonly string TransactionLogFileNameKey = "TransactionLogFileName";

                public static readonly string VideoDirKey = "VideoDir";

                public static readonly string VideoFileNameKey = "VideoFileName";

                public static readonly string PreviewsDirKey = "PreviewsDir";

                public static readonly string ThumbnailFileNameKey = "ThumbnailFileName";

                public static readonly string ProjectZipNameKey = "ProjectZipName";

                public static readonly string ProjectJsonNameKey = "ProjectJsonName";

                public static readonly string ProjectResourceDirKey = "ProjectResourceDir";

                public static readonly string ProjectResourceFileNameKey = "ProjectResourceFileName";

                public static readonly string ThumbnailsDirKey = "ThumbnailsDir";

                public static readonly string TranscodeStateFileNameKey = "TranscodeStateFileName";
            }

            public static class Urls
            {
                public static readonly string ProjectHostKey = "ProjectHost";

                public static readonly string ProjectPathKey = "ProjectPath";

                public static readonly string ProjectTransactionPathKey = "ProjectTransactionPath";

                public static readonly string ProjectVideoPathKey = "ProjectVideoPath";

                public static readonly string ProjectPreviewPathKey = "ProjectPreviewPath";

                public static readonly string ProjectThumbnailPathKey = "ProjectThumbnailPath";

                public static readonly string ProjectZipPathKey = "ProjectZipPath";

                public static readonly string ProjectJsonPathKey = "ProjectJsonPath";

                public static readonly string ProjectResourcePathKey = "ProjectResourcePath";
            }
        }
    }
}
