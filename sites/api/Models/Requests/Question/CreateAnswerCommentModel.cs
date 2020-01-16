using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateAnswerCommentModel : CreateCommentModel<AnswerComment>
    {
        [Required]
        public Guid TargetAnswerId { get; set; }

        public override AnswerComment Create()
        {
            var answerComment = BaseCreate();
            answerComment.TargetId = TargetAnswerId;
            return answerComment;
        }
    }
}