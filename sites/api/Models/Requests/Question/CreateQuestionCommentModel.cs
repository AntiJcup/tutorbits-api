using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateQuestionCommentModel : CreateCommentModel<QuestionComment>
    {
        [Required]
        public Guid TargetQuestionId { get; set; }

        public override QuestionComment Create()
        {
            var questionComment = BaseCreate();
            questionComment.TargetId = TargetQuestionId;
            return questionComment;
        }
    }
}