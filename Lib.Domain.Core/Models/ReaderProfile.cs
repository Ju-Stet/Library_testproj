using System.ComponentModel.DataAnnotations;

namespace Lib.Domain.Core.Models
{
    public class ReaderProfile
    {
        [Key]
        public int ReaderId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Reader Reader { get; set; }
    }
}
