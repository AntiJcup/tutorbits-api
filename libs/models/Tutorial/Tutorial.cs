using System;

namespace Common
{
    public class Tutorial
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
