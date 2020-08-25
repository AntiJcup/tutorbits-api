using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialCommentModel : CreateCommentModel<TutorialComment>
    {
        public override TutorialComment Create()
        {
            return BaseCreate();
        }
    }
}