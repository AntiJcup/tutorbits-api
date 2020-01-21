using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Examples")]
    public class Example : BaseModel
    {
        [MaxLength(64)]
        [Index]
        public string Title { get; set; }

        public ProgrammingTopic ProgrammingTopic { get; set; }

        [MaxLength(1028)]
        public string Description { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? ThumbnailId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("ThumbnailId")]
        public virtual Thumbnail Thumbnail { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<ExampleRating> Ratings { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<ExampleComment> Comments { get; set; }

    }
}
