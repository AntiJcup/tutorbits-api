using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class ExampleCommentRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual ExampleComment Target { get; set; }
    }
}