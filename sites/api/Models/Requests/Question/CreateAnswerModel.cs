using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerModel : CreateCommentModel<Answer>
    {
        [Required]
        public Guid TargetQuestionId { get; set; }

        public override Answer Create()
        {
            var answer = BaseCreate();
            answer.TargetId = TargetQuestionId;
            return answer;
        }
    }
}