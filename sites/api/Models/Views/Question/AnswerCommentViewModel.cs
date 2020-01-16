using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class AnswerCommentViewModel : CommentViewModel<AnswerComment>
    {
        public override void Convert(AnswerComment baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}