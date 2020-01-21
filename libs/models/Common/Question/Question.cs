using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Questions")]
    public class Question : BaseModel
    {
        [MaxLength(256)]
        [Index]
        public string Title { get; set; }

        public ProgrammingTopic ProgrammingTopic { get; set; }

        [MaxLength(2056)]
        public string Description { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<QuestionRating> Ratings { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<QuestionComment> Comments { get; set; }

        [InverseProperty("Target")]
        public virtual ICollection<Answer> Answers { get; set; }

    }
}
