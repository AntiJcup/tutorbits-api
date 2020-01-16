using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class QuestionCommentViewModel : CommentViewModel<QuestionComment>
    {
        public override void Convert(QuestionComment baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}