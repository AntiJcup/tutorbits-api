using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    [Table("Comments")]
    public class Comment : BaseModel
    {
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1028)]
        public string Body { get; set; }

        public CommentType CommentType { get; set; }

        public Guid TargetId { get; set; }

        [ForeignKey("TargetId")]
        public virtual Tutorial Target { get; set; }
    }
}