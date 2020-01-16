using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateQuestionModel : BaseUpdateModel<Question>
    {
        [Required]
        public Guid Id { get; set; }

        [MinLength(4)]
        [MaxLength(256)]
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(2056)]
        public string Description { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Question model)
        {
            model.Title = Title;
            model.Description = Description;
        }
    }
}