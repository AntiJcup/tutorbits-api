using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class AnswerRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual Answer Target { get; set; }
    }
}