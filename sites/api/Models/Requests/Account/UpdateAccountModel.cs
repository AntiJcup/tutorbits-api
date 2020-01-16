using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class UpdateAccountModel : BaseUpdateModel<Account>
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(4)]
        [RegularExpression("^[a-zA-Z0-9._-]+$")]
        public string NickName { get; set; }

        public bool? AcceptOffers { get; set; }

        public override ICollection<object> GetKeys()
        {
            return new List<object> {
                Id
            };
        }

        public override void Update(Account model)
        {
            model.NickName = NickName;
            model.AcceptOffers = AcceptOffers.HasValue ? AcceptOffers.Value : model.AcceptOffers;
        }
    }
}