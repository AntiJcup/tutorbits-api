using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialRatingModel : CreateRatingModel<TutorialRating>
    {
        public override TutorialRating Create()
        {
            return BaseCreate();
        }
    }
}