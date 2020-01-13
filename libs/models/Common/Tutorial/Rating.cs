using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    [Table("Ratings")]
    public class Rating : BaseModel
    {
        [Range(0, 5)]
        public int Score { get; set; }

        public Guid TargetId { get; set; }
        
        [ForeignKey("TargetId")]
        public virtual Tutorial Target { get; set; }
    }
}