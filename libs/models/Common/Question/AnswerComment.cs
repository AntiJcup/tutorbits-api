using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class AnswerComment : Comment
    {
        public Guid? TargetId { get; set; }

        [ForeignKey("TargetId")]
        public virtual Answer Target { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<AnswerCommentRating> Ratings { get; set; }
    }
}