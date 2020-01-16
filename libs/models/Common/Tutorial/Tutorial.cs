using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Tutorials")]
    public class Tutorial : BaseModel
    {
        [MaxLength(64)]
        [Index]
        public string Title { get; set; }

        public TutorialTopics TutorialTopic { get; set; }

        [MaxLength(1028)]
        public string Description { get; set; }

        public UInt64 DurationMS { get; set; }

        public TutorialCategory TutorialCategory { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<TutorialRating> Ratings { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<TutorialComment> Comments { get; set; }

    }
}
