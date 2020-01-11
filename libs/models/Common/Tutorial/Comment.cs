using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    [Table("Comments")]
    public class Comment : BaseModel
    {
        public int Title { get; set; }

        public int Body { get; set; }

        public CommentType CommentType { get; set; }

        public Guid TargetId { get; set; }

        [ForeignKey("TargetId")]
        public virtual Tutorial Target { get; set; }
    }
}