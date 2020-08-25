using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionCommentModel : CreateCommentModel<QuestionComment>
    {
        public override QuestionComment Create()
        {
            return BaseCreate();
        }
    }
}