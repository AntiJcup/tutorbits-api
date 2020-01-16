using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialRatingModel : CreateRatingModel<TutorialRating>
    {
        [Required]
        public Guid TargetTutorialId { get; set; }

        public override TutorialRating Create()
        {
            var tutorialRating = BaseCreate();
            tutorialRating.TargetId = TargetTutorialId;
            return tutorialRating;
        }
    }
}