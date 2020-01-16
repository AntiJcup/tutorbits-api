using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateAnswerCommentRatingModel : UpdateRatingModel<AnswerCommentRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(AnswerCommentRating model)
        {
            BaseUpdate(model);
        }
    }
}