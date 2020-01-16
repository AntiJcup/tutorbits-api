using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class QuestionRatingViewModel : RatingViewModel<QuestionRating>
    {
        public override void Convert(QuestionRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}