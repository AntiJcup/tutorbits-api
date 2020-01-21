using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class ExampleCommentRatingViewModel : RatingViewModel<ExampleCommentRating>
    {
        public override void Convert(ExampleCommentRating baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}