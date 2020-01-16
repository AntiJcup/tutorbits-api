using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialRatingModel : CreateRatingModel<TutorialRating>
    {
        public override TutorialRating Create()
        {
            var tutorialRating = BaseCreate();
            return tutorialRating;
        }
    }
}