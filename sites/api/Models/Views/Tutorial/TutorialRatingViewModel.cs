using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class TutorialRatingViewModel : RatingViewModel<TutorialRating>
    {
        public override void Convert(TutorialRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}