using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class TutorialViewModel : BaseViewModel<Tutorial>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string ProjectId { get; set; }

        public string VideoId { get; set; }

        public string ThumbnailId { get; set; }

        public UInt64 DurationMS { get; set; } //Enriched

        public string ThumbnailUrl { get; set; } //Enriched

        public int Score { get; set; } = 1; //Enriched

        public int CommentCount { get; set; } = 0; //Enriched

        public override void Convert(Tutorial baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Topic = baseModel.ProgrammingTopic.ToString();
            Description = baseModel.Description;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            OwnerId = baseModel.OwnerAccountId.ToString();
            ProjectId = baseModel.ProjectId?.ToString();
            VideoId = baseModel.VideoId?.ToString();
            ThumbnailId = baseModel.ThumbnailId?.ToString();
            DateCreated = baseModel.DateCreated.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }
    }
}