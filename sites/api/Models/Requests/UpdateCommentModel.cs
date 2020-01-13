using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateCommentModel : BaseUpdateModel<Comment>
    {
        [Required]
        public Guid Id { get; set; }

        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(1028)]
        public string Body { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Comment model)
        {
            model.Title = Title;
            model.Body = Body;
        }
    }
}