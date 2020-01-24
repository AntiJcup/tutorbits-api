using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    public class Project : BaseModel
    {
        public UInt64 DurationMS { get; set; }

        [InverseProperty("Project")]
        public virtual ICollection<Tutorial> Tutorials { get; set; }

        [InverseProperty("Project")]
        public virtual ICollection<Example> Examples { get; set; }
    }
}