using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateThumbnailModel : BaseUpdateModel<Thumbnail>
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public IFormFile Thumbnail { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Thumbnail model)
        {
        }
    }
}