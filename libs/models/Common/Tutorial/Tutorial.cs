using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits.Models.Common
{
    [Table("Tutorials")]
    public class Tutorial : Base
    {
        public Guid? UserId { get; set; }

        public string Name { get; set; }
    }
}
