using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionCommentRatingModel : CreateRatingModel<QuestionCommentRating>
    {
        [Required]
        public Guid TargetCommentId { get; set; }

        public override QuestionCommentRating Create()
        {
            var questionCommentRating = BaseCreate();
            questionCommentRating.TargetId = TargetCommentId;
            return questionCommentRating;
        }
    }
}