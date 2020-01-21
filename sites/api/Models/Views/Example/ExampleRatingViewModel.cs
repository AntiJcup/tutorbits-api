using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class ExampleRatingViewModel : RatingViewModel<ExampleRating>
    {
        public override void Convert(ExampleRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}