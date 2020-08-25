using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateExampleRatingModel : CreateRatingModel<ExampleRating>
    {
        public override ExampleRating Create()
        {
            return BaseCreate();
        }
    }
}