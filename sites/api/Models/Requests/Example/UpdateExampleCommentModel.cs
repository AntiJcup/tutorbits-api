using System.Collections.Generic;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateExampleCommentModel : UpdateCommentModel<ExampleComment>
    {
        public override ICollection<object> GetKeys()
        {
            return BaseGetKeys();
        }

        public override void Update(ExampleComment model)
        {
            BaseUpdate(model);
        }
    }
}