using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class ExampleComment : Comment
    {
        [ForeignKey("TargetId")]
        public virtual Example Target { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<ExampleCommentRating> Ratings { get; set; }
    }
}