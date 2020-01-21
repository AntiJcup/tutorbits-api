using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class VideoViewModel : BaseViewModel<Video>
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public string Url { get; set; } //Enriched

        public override void Convert(Video baseModel)
        {
            Id = baseModel.Id.ToString();
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
        }
    }
}