using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    public class Thumbnail : BaseModel
    {
        [InverseProperty("Thumbnail")]
        public virtual ICollection<Tutorial> Tutorials { get; set; }
    }
}