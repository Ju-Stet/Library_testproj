using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lib.Domain.Core.Models
{
    public class Reader : BaseEntity
    {
        [Key]
        public override int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Card> Cards { get; set; }
        public ReaderProfile ReaderProfile { get; set; }
    }
}
