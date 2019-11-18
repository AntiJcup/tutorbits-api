using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Tutorials")]
    public class Tutorial : BaseModel
    {
        [MaxLength(64)]
        public string Title { get; set; }

        [MaxLength(64)]
        public string Language { get; set; }

        [MaxLength(1028)]
        public string Description { get; set; }
    }
}
