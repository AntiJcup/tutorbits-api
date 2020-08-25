using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionCommentRatingModel : CreateRatingModel<QuestionCommentRating>
    {
        public override QuestionCommentRating Create()
        {
            return BaseCreate();
        }
    }
}