using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class AnswerViewModel : BaseViewModel<Answer>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public override void Convert(Answer baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Body = baseModel.Body;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
        }
    }
}