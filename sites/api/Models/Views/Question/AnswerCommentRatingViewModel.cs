using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class AnswerCommentRatingViewModel : RatingViewModel<AnswerCommentRating>
    {
        public override void Convert(AnswerCommentRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}