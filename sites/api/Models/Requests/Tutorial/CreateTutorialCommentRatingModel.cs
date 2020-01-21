using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialCommentRatingModel : CreateRatingModel<TutorialCommentRating>
    {
        public override TutorialCommentRating Create()
        {
            return BaseCreate();
        }
    }
}