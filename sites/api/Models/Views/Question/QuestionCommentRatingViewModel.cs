using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class QuestionCommentRatingViewModel : RatingViewModel<QuestionCommentRating>
    {
        public override void Convert(QuestionCommentRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}