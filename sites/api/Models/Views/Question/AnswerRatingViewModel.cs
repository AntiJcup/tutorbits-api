using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class AnswerRatingViewModel : RatingViewModel<AnswerRating>
    {
        public override void Convert(AnswerRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}