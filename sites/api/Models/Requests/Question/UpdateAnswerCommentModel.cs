using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateAnswerCommentModel : UpdateCommentModel<AnswerComment>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(AnswerComment model)
        {
            BaseUpdate(model);
        }
    }
}