using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateTutorialModel : BaseUpdateModel<Tutorial>
    {
        [Required]
        public Guid Id { get; set; }

        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(1028)]
        public string Description { get; set; }

        public string ThumbnailId { get; set; }

        public string VideoId { get; set; }

        public string ProjectId { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Tutorial model)
        {
            model.Title = Title;
            model.Description = Description;

            model.ThumbnailId = string.IsNullOrWhiteSpace(ThumbnailId) ? model.ProjectId : Guid.Parse(ThumbnailId);

            if (model.Status == BaseState.Inactive)
            {
                model.VideoId = string.IsNullOrWhiteSpace(VideoId) ? model.VideoId : Guid.Parse(VideoId);
                model.ProjectId = string.IsNullOrWhiteSpace(VideoId) ? model.ProjectId : Guid.Parse(ProjectId);
            }
        }
    }
}