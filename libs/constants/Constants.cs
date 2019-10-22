using System;

public static class Constants
{
    public static class Configuration
    {
        public static class Sections
        {
            public static readonly string SettingsKey = "Settings";
            public static readonly string UrlsKey = "Urls";

            public static class Settings
            {
                public static readonly string PartitionSizeKey = "PartitionSize";
            }

            public static class Urls
            {
                public static readonly string ProjectHostKey = "ProjectHost";

                public static readonly string ProjectTransactionPathKey = "ProjectTransactionPath";
            }
        }
    }
}
