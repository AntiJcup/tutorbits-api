using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class AnswerCommentRating : Rating
    {
        public Guid TargetId { get; set; }
        
        [ForeignKey("TargetId")]
        public virtual AnswerComment Target { get; set; }
    }
}