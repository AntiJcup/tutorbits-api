using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialCommentRatingModel : CreateRatingModel<TutorialCommentRating>
    {
        [Required]
        public Guid TargetCommentId { get; set; }

        public override TutorialCommentRating Create()
        {
            var tutorialCommentRating = BaseCreate();
            tutorialCommentRating.TargetId = TargetCommentId;
            return tutorialCommentRating;
        }
    }
}