using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateProjectModel : BaseUpdateModel<Project>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public UInt64 DurationMS { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Project model)
        {
            model.DurationMS = DurationMS;
        }
    }
}