using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class TutorialRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual Tutorial Target { get; set; }
    }
}