using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    public class Video : BaseModel
    {
        public UInt64 DurationMS { get; set; }

        [InverseProperty("Video")]
        public virtual ICollection<Tutorial> Tutorials { get; set; }
    }
}