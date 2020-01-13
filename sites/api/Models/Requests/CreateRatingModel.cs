using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateRatingModel : BaseCreateModel<Rating>
    {
        [Range(0, 5)]
        public int Score { get; set; }

        [Required]
        public Guid TargetTutorialId { get; set; }

        public override Rating Create()
        {
            return new Rating()
            {
                Score = Score,
                TargetId = TargetTutorialId
            };
        }
    }
}