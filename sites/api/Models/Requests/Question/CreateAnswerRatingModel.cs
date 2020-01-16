using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerRatingModel : CreateRatingModel<AnswerRating>
    {
        [Required]
        public Guid TargetAnswerId { get; set; }

        public override AnswerRating Create()
        {
            var answerRating = BaseCreate();
            answerRating.TargetId = TargetAnswerId;
            return answerRating;
        }
    }
}