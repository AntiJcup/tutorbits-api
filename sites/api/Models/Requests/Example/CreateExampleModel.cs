using System;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateExampleModel : BaseCreateModel<Example>
    {
        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        [MaxLength(1028)]
        public string Description { get; set; }

        public override Example Create()
        {
            return new Example()
            {
                Title = Title,
                ProgrammingTopic = (ProgrammingTopic)Enum.Parse(typeof(ProgrammingTopic), Topic),
                Description = Description
            };
        }
    }
}