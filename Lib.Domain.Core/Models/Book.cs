using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lib.Domain.Core.Models
{
    public class Book : BaseEntity
    {
        [Key]
        public override int Id { get; set; }

        [DataType(DataType.Text)]
        public string Author { get; set; }

        [DataType(DataType.Text)]
        public string Title { get; set; }
        public int Year { get; set; }

        public ICollection<History> Cards { get; set; }
    }
}
