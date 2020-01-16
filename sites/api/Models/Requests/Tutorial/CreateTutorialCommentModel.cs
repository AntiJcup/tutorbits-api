using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TutorBits.Models.Common;

namespace api.Models.Requests
{
    public class CreateTutorialCommentModel : CreateCommentModel<TutorialComment>
    {
        [Required]
        public Guid TargetTutorialId { get; set; }

        public override TutorialComment Create()
        {
            var tutorialComment = BaseCreate();
            tutorialComment.TargetId = TargetTutorialId;
            return tutorialComment;
        }
    }
}