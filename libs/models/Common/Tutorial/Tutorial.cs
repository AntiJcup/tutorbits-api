using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits
{
    namespace Models
    {
        namespace Common
        {
            [Table("Tutorials")]
            public class Tutorial : Base
            {
                public Guid? UserId { get; set; }

                public string Name { get; set; }
            }
        }
    }
}