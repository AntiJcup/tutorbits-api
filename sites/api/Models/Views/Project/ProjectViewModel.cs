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

        public override void Convert(Project baseModel)
        {
            Id = baseModel.Id.ToString();
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            DurationMS = baseModel.DurationMS;
        }
    }
}