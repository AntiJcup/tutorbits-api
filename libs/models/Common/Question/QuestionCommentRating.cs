using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class QuestionCommentRating : Rating
    {
        public Guid TargetId { get; set; }
        
        [ForeignKey("TargetId")]
        public virtual QuestionComment Target { get; set; }
    }
}