using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    public class Account : BaseModel
    {
        [Required]
        [EmailAddress]
        public string Email;

        [Required]
        [MaxLength(256)]
        [MinLength(4)]
        public string NickName;

        public bool AcceptOffers;
    }
}