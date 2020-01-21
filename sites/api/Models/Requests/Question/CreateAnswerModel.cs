using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerModel : CreateCommentModel<Answer>
    {
        public override Answer Create()
        {
            return BaseCreate();
        }
    }
}