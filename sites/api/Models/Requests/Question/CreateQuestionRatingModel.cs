using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionRatingModel : CreateRatingModel<QuestionRating>
    {
        public override QuestionRating Create()
        {
            var questionRating = BaseCreate();
            return questionRating;
        }
    }
}