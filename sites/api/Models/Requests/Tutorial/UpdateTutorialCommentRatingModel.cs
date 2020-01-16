using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateTutorialCommentRatingModel : UpdateRatingModel<TutorialCommentRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(TutorialCommentRating model)
        {
            BaseUpdate(model);
        }
    }
}