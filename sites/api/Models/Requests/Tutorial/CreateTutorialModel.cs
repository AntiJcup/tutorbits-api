using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialModel : BaseCreateModel<Tutorial>
    {
        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        [MaxLength(1028)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        public override Tutorial Create()
        {
            return new Tutorial()
            {
                Title = Title,
                TutorialLanguage = (TutorialLanguage)Enum.Parse(typeof(TutorialLanguage), Language),
                Description = Description,
                TutorialCategory = (TutorialCategory)Enum.Parse(typeof(TutorialCategory), Category),
            };
        }
    }
}