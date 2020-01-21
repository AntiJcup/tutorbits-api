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

        public ProgrammingTopic ProgrammingTopic { get; set; }

        [MaxLength(1028)]
        public string Description { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? VideoId { get; set; }

        public Guid? ThumbnailId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; }

        [ForeignKey("ThumbnailId")]
        public virtual Thumbnail Thumbnail { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<TutorialRating> Ratings { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<TutorialComment> Comments { get; set; }

    }
}
