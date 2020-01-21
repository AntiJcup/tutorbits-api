using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateExampleRatingModel : UpdateRatingModel<ExampleRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(ExampleRating model)
        {
            BaseUpdate(model);
        }
    }
}