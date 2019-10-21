using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits
{
    namespace Models
    {
        namespace Common
        {
            [Table("Users")]
            public class User : Base
            {
                public string Name;

                [Required]
                [MaxLength(100)]
                [MinLength(5)]
                public string Email;
            }
        }
    }
}