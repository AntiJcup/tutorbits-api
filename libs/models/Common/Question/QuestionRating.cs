using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class QuestionRating : Rating
    {
        public Guid TargetId { get; set; }
        
        [ForeignKey("TargetId")]
        public virtual Question Target { get; set; }
    }
}