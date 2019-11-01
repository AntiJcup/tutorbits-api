using System;

public static class Constants
{
    public static class Configuration
    {
        public static class Sections
        {
            public static readonly string SettingsKey = "Settings";

            public static readonly string PathsKey = "Paths";

            public static readonly string UrlsKey = "Urls";

            public static class Settings
            {
                public static readonly string PartitionSizeKey = "PartitionSize";
            }

            public static class Paths
            {
                public static readonly string ProjectsDirKey = "ProjectsDir";

                public static readonly string ProjectFileNameKey = "ProjectFileName";

                public static readonly string TransactionsDirKey = "TransactionsDir";

                public static readonly string TransactionLogFileNameKey = "TransactionLogFileName";

                public static readonly string VideosDirKey = "VideosDir";

                public static readonly string VideoFileNameKey = "VideoFileName";
            }

            public static class Urls
            {
                public static readonly string ProjectHostKey = "ProjectHost";

                public static readonly string ProjectPathKey = "ProjectPath";

                public static readonly string ProjectTransactionPathKey = "ProjectTransactionPath";

                public static readonly string ProjectVideoPathKey = "ProjectVideoPath";
            }
        }
    }
}
