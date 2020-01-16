using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public abstract class CreateRatingModel<TRatingType> : BaseCreateModel<TRatingType> where TRatingType : Rating, new()
    {
        [Range(0, 5)]
        public int Score { get; set; }

        protected TRatingType BaseCreate()
        {
            return new TRatingType()
            {
                Score = Score,
            };
        }
    }
}