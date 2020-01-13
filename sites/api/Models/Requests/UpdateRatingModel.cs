using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateRatingModel : BaseUpdateModel<Rating>
    {
        [Required]
        public Guid Id { get; set; }

        [Range(0, 5)]
        public int Score { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Rating model)
        {
            model.Score = Score;
        }
    }
}