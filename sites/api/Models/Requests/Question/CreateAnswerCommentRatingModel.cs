using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerCommentRatingModel : CreateRatingModel<AnswerCommentRating>
    {
        public override AnswerCommentRating Create()
        {
            return BaseCreate();
        }
    }
}