using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerCommentRatingModel : CreateRatingModel<AnswerCommentRating>
    {
        [Required]
        public Guid TargetCommentId { get; set; }

        public override AnswerCommentRating Create()
        {
            var answerCommentRating = BaseCreate();
            answerCommentRating.TargetId = TargetCommentId;
            return answerCommentRating;
        }
    }
}