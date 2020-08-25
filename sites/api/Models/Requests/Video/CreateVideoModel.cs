using api.Models.Updates;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateVideoModel : BaseCreateModel<Video>
    {
        public override Video Create()
        {
            return new Video()
            {
            };
        }
    }
}