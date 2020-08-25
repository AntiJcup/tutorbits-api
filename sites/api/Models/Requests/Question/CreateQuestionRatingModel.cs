using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionRatingModel : CreateRatingModel<QuestionRating>
    {
        public override QuestionRating Create()
        {
            return BaseCreate();
        }
    }
}