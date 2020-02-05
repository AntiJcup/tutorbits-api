using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class AnswerViewModel : BaseViewModel<Answer>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Status { get; set; }

        public int Score { get; set; } = 1; //Enriched

        public override void Convert(Answer baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Body = baseModel.Body;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            OwnerId = baseModel.OwnerAccountId.ToString();
            DateCreated = baseModel.DateCreated.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }
    }
}