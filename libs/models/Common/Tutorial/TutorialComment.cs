using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class TutorialComment : Comment
    {
        [ForeignKey("TargetId")]
        public virtual Tutorial Target { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<TutorialCommentRating> Ratings { get; set; }
    }
}