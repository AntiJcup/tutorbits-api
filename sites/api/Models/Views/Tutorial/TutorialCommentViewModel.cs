using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class TutorialCommentViewModel : CommentViewModel<TutorialComment>
    {
        public override void Convert(TutorialComment baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}