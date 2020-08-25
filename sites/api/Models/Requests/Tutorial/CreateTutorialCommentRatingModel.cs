using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialCommentRatingModel : CreateRatingModel<TutorialCommentRating>
    {
        public override TutorialCommentRating Create()
        {
            return BaseCreate();
        }
    }
}