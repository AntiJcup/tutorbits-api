using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateUpdateTutorialModel : BaseConvertableModel<Tutorial>
    {
        public string Id { get; set; }

        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        public TutorialLanguage Language { get; set; }

        public string Description { get; set; }

        public override Tutorial Convert()
        {
            return new Tutorial()
            {
                Title = Title,
                TutorialLanguage = Language,
                Description = Description
            };
        }
    }
}