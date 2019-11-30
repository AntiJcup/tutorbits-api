using System;
using System.ComponentModel.DataAnnotations;
using TutorBits.Models.Common;

namespace api.Models.Views
{
    public class AccountViewModel : BaseViewModel<Account>
    {
        public string UserId { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

        public DateTime AccountCreated { get; set; }

        public override void Convert(Account baseModel)
        {
            UserId = baseModel.Owner;
            NickName = baseModel.NickName;
            Email = baseModel.Email;
            AccountCreated = baseModel.DateCreated;
        }
    }
}