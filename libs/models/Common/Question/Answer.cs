using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class Answer : Comment
    {
        public Guid? TargetId { get; set; }

        [ForeignKey("TargetId")]
        public virtual Question Target { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<AnswerRating> Ratings { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<AnswerComment> Comments { get; set; }
    }
}