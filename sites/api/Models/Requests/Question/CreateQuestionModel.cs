using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionModel : BaseCreateModel<Question>
    {
        [MinLength(4)]
        [MaxLength(256)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        [MaxLength(2056)]
        public string Description { get; set; }

        public override Question Create()
        {
            return new Question()
            {
                Title = Title,
                TutorialLanguage = (TutorialLanguage)Enum.Parse(typeof(TutorialLanguage), Language),
                Description = Description,
            };
        }
    }
}