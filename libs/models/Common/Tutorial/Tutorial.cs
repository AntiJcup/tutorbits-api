using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Tutorials")]
    public class Tutorial : BaseModel
    {
        public Guid? UserId { get; set; }

        public string Title { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }
    }
}
