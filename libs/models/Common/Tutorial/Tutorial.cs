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

        public TutorialLanguage TutorialLanguage { get; set; }

        [MaxLength(1028)]
        public string Description { get; set; }

        public UInt64 DurationMS { get; set; }

        public TutorialCategory TutorialCategory { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<Rating> Ratings { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
