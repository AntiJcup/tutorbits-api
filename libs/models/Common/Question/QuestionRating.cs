using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class QuestionRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual Question Target { get; set; }
    }
}