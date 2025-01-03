using System;
using System.ComponentModel.DataAnnotations;

namespace TutorBits.Models.Common
{
    public class Rating : BaseModel
    {
        [Required]
        public Guid TargetId { get; set; }
        
        [Range(-1, 1)]
        public int Score { get; set; }
    }
}