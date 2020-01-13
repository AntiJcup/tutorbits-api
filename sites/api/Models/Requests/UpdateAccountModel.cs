using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateAccountModel : BaseUpdateModel<Account>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(4)]
        public string NickName { get; set; }

        public bool AcceptOffers { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Account model)
        {
            model.NickName = NickName;
            model.AcceptOffers = AcceptOffers;
        }
    }
}