using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateThumbnailModel : BaseCreateModel<Thumbnail>
    {
        [Required]
        public IFormFile Thumbnail { get; set; }

        public override Thumbnail Create()
        {
            return new Thumbnail()
            {
            };
        }
    }
}