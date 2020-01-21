using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerRatingModel : CreateRatingModel<AnswerRating>
    {
        public override AnswerRating Create()
        {
            return BaseCreate();
        }
    }
}