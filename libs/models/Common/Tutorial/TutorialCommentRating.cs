using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class TutorialCommentRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual TutorialComment Target { get; set; }
    }
}