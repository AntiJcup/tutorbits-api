using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class ExampleRating : Rating
    {
        [ForeignKey("TargetId")]
        public virtual Example Target { get; set; }
    }
}