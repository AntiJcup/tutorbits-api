using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerCommentModel : CreateCommentModel<AnswerComment>
    {
        public override AnswerComment Create()
        {
            return BaseCreate();
        }
    }
}