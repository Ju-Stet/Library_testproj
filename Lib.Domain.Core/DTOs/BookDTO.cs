using System.Collections.Generic;

namespace Lib.Domain.Core.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string Author { get; set; }

        public ICollection<int> CardsIds { get; set; }

    }
}
