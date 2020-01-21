using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class QuestionViewModel : BaseViewModel<Question>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public UInt64 DurationMS { get; set; }

        public override void Convert(Question baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Topic = baseModel.ProgrammingTopic.ToString();
            Description = baseModel.Description;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
        }
    }
}