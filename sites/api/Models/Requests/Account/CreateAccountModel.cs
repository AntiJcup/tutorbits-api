using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAccountModel : BaseCreateModel<Account>
    {
        [Required]
        [MaxLength(256)]
        [MinLength(4)]
        public string NickName { get; set; }

        public bool AcceptOffers { get; set; }

        public override Account Create()
        {
            return new Account()
            {
                NickName = NickName,
                AcceptOffers = AcceptOffers
            };
        }
    }
}