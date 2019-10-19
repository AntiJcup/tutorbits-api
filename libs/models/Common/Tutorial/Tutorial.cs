using System;
using System.ComponentModel.DataAnnotations;

namespace TutorBits
{
    namespace Models
    {
        namespace Common
        {
            public class Tutorial
            {
                [Key]
                public Guid Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}