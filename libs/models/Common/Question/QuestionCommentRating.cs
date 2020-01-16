using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class QuestionCommentRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual QuestionComment Target { get; set; }
    }
}