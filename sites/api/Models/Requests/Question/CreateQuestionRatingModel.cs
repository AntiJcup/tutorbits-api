using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionRatingModel : CreateRatingModel<QuestionRating>
    {
        [Required]
        public Guid TargetQuestionId { get; set; }

        public override QuestionRating Create()
        {
            var questionRating = BaseCreate();
            questionRating.TargetId = TargetQuestionId;
            return questionRating;
        }
    }
}