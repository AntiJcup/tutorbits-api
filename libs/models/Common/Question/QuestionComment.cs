using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class QuestionComment : Comment
    {
        [ForeignKey("TargetId")]
        public virtual Question Target { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<QuestionCommentRating> Ratings { get; set; }
    }
}