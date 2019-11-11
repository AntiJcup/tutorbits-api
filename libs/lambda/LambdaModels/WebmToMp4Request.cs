using Amazon;

namespace LambdaModels
{
    public class WebmToMp4Request
    {
        public string BucketName;

        public string Endpoint;

        public string WebmPath;
        public string Mp4Path;
    }
}