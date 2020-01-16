using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public abstract class UpdateRatingModel<TRatingType> : BaseUpdateModel<TRatingType> where TRatingType : Rating, new()
    {
        [Required]
        public Guid Id { get; set; }

        [Range(-1, 1)]
        public int Score { get; set; }

        protected ICollection<object> BaseGetKeys()
        {
            return new List<object> {
                Id
            };
        }

        protected void BaseUpdate(TRatingType model)
        {
            model.Score = Score;
        }
    }
}