using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class ExampleCommentViewModel : CommentViewModel<ExampleComment>
    {
        public override void Convert(ExampleComment baseModel)
        {
            BaseConvert(baseModel);
        }
    }
}