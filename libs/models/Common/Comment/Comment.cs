using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TutorBits.Models.Common
{
    public class Comment : BaseModel
    {
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1028)]
        public string Body { get; set; }
    }
}