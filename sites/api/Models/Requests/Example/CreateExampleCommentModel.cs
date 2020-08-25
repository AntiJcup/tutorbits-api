using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateExampleCommentModel : CreateCommentModel<ExampleComment>
    {
        public override ExampleComment Create()
        {
            return BaseCreate();
        }
    }
}