using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateAnswerModel : UpdateCommentModel<Answer>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(Answer model)
        {
            BaseUpdate(model);
        }
    }
}