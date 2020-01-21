using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionCommentModel : CreateCommentModel<QuestionComment>
    {
        public override QuestionComment Create()
        {
            return BaseCreate();
        }
    }
}