using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateExampleCommentRatingModel : UpdateRatingModel<ExampleCommentRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(ExampleCommentRating model)
        {
            BaseUpdate(model);
        }
    }
}