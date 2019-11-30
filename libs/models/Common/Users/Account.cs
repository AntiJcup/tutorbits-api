using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Accounts")]
    public class Account : BaseModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(1028)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(4)]
        public string NickName { get; set; }

        public bool AcceptOffers { get; set; }
    }
}