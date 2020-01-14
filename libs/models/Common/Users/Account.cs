using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Accounts")]
    public class Account : BaseModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(512)]
        [Index]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(4)]
        [Index]
        [RegularExpression("^[a-zA-Z0-9._-]+$")]
        public string NickName { get; set; }

        public bool? AcceptOffers { get; set; }
    }
}