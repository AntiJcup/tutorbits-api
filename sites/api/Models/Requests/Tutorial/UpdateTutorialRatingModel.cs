using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateTutorialRatingModel : UpdateRatingModel<TutorialRating>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(TutorialRating model)
        {
            BaseUpdate(model);
        }
    }
}