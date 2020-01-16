using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerCommentRatingModel : CreateRatingModel<AnswerCommentRating>
    {
        public override AnswerCommentRating Create()
        {
            var answerCommentRating = BaseCreate();
            return answerCommentRating;
        }
    }
}