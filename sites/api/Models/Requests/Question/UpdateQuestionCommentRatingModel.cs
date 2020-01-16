using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateQuestionCommentRatingModel : UpdateRatingModel<QuestionCommentRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(QuestionCommentRating model)
        {
            BaseUpdate(model);
        }
    }
}