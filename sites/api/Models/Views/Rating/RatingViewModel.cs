using System;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public abstract class RatingViewModel<TRatingType> : BaseViewModel<TRatingType> where TRatingType : Rating, new()
    {
        public string Id { get; set; }

        public int Score { get; set; }

        public string Status { get; set; }

        protected void BaseConvert(Rating baseModel)
        {
            Id = baseModel.Id.ToString();
            Score = baseModel.Score;
            Status = baseModel.Status.ToString();
            Owner = baseModel.Owner;
            OwnerId = baseModel.OwnerAccountId.ToString();
            DateCreated = baseModel.DateCreated.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }
    }
}