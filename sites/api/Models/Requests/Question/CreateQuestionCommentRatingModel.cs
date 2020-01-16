using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionCommentRatingModel : CreateRatingModel<QuestionCommentRating>
    {
        public override QuestionCommentRating Create()
        {
            var questionCommentRating = BaseCreate();
            return questionCommentRating;
        }
    }
}