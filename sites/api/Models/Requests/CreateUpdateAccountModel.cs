using System.ComponentModel.DataAnnotations;
using api.Models.Updates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateUpdateAccountModel : BaseConvertableModel<Account>
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public bool AcceptOffers { get; set; }
        
        public override Account Convert()
        {
            return new Account();
        }
    }
}