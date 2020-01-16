using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateQuestionCommentModel : UpdateCommentModel<QuestionComment>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(QuestionComment model)
        {
            BaseUpdate(model);
        }
    }
}