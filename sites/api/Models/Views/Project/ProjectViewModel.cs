using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class ProjectViewModel : BaseViewModel<Project>
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public UInt64 DurationMS { get; set; }

        public string Url { get; set; } //Enriched

        public string Type { get; set; }

        public override void Convert(Project baseModel)
        {
            Id = baseModel.Id.ToString();
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            OwnerId = baseModel.OwnerAccountId.ToString();
            DurationMS = baseModel.DurationMS;
            Type = baseModel.ProjectType.ToString();
            DateCreated = baseModel.DateCreated.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }
    }
}