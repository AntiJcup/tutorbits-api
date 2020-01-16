using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class TutorialCommentRatingViewModel : RatingViewModel<TutorialCommentRating>
    {
        public override void Convert(TutorialCommentRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}