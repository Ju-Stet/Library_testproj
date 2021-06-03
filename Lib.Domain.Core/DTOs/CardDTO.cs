using System;
using System.Collections.Generic;

namespace Lib.Domain.Core.DTOs
{
    public class CardDTO
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public int ReaderId { get; set; }

        public ICollection<int> BooksIds { get; set; }

    }
}
