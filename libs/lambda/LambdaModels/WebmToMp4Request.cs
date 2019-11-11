using Amazon;

namespace LambdaModels
{
    public class WebmToMp4Request
    {
        public string BucketName;

        public RegionEndpoint Endpoint;

        public string WebmPath;
        public string Mp4Path;
    }
}