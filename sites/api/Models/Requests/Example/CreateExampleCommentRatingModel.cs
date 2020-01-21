using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateExampleCommentRatingModel : CreateRatingModel<ExampleCommentRating>
    {
        public override ExampleCommentRating Create()
        {
            return BaseCreate();
        }
    }
}