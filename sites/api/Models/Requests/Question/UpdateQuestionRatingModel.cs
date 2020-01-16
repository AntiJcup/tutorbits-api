using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateQuestionRatingModel : UpdateRatingModel<QuestionRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(QuestionRating model)
        {
            BaseUpdate(model);
        }
    }
}