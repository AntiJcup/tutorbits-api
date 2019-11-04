using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBits
{
    namespace Models
    {
        namespace Common
        {
            public enum BaseState
            {
                Active,
                Inactive,
                Deleted,
            }

            public abstract class Base
            {
                [Key]
                [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public Guid Id { get; set; }


                [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public DateTime DateCreated { get; set; }

                [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
                public DateTime DateModified { get; set; }

                public string Notes { get; set; }

                public BaseState Status { get; set; }
            }
        }
    }
}