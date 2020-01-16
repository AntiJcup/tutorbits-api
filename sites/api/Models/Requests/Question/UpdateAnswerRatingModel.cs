using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateAnswerRatingModel : UpdateRatingModel<AnswerRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(AnswerRating model)
        {
            BaseUpdate(model);
        }
    }
}