using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lib.Domain.Core.Models
{
    public class Card : BaseEntity
    {
        [Key]
        public override int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public int ReaderId { get; set; }

        public ICollection<History> Books { get; set; }
        public Reader Reader { get; set; }
    }
}
