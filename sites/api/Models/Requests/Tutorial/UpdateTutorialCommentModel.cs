using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateTutorialCommentModel : UpdateCommentModel<TutorialComment>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(TutorialComment model)
        {
            BaseUpdate(model);
        }
    }
}