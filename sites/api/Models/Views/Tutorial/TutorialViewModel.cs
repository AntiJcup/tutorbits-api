using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class TutorialViewModel : BaseViewModel<Tutorial>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public UInt64 DurationMS { get; set; }

        public string ThumbnailUrl { get; set; } //Enriched

        public string Category { get; set; }

        public override void Convert(Tutorial baseModel)
        {
            Id = baseModel.Id.ToString();
            Title = baseModel.Title;
            Language = baseModel.TutorialLanguage.ToString();
            Description = baseModel.Description;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            DurationMS = baseModel.DurationMS;
            Category = baseModel.TutorialCategory.ToString();
        }
    }
}