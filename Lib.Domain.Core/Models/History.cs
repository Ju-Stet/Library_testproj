using System;
using System.ComponentModel.DataAnnotations;

namespace Lib.Domain.Core.Models
{
    public class History : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        public int CardId { get; set; }
        public int BookId { get; set; }

        [DataType(DataType.Date)]
        public DateTime TakeDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        public Card Card { get; set; }
        public Book Book { get; set; }
    }
}
