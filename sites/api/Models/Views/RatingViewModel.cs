using System;
using System.ComponentModel.DataAnnotations;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class RatingViewModel : BaseViewModel<Rating>
    {
        public string Id { get; set; }

        public int Score { get; set; }

        public string Status { get; set; }

        public override void Convert(Rating baseModel)
        {
            Id = baseModel.Id.ToString();
            Score = baseModel.Score;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
        }
    }
}