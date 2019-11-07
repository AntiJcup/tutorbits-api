using System.ComponentModel.DataAnnotations;

namespace api.Models.Requests
{
    public class CreateTutorial
    {
        [MinLength(4)]
        [MaxLength(64)]
        [Required]
        public string Title { get; set; }

        [MinLength(1)]
        [MaxLength(64)]
        [Required]
        public string Language { get; set; }

        public string Description { get; set; }
    }
}