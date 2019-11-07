using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Users")]
    public class User : BaseModel
    {
        public string Name;

        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Email;
    }
}