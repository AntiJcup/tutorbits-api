using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class TutorialCommentRating : Rating
    {
        public Guid TargetId { get; set; }
        
        [ForeignKey("TargetId")]
        public virtual TutorialComment Target { get; set; }
    }
}