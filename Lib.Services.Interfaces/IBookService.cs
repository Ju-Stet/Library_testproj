using Lib.Domain.Core.DTOs;
using System.Collections.Generic;

namespace Lib.Services.Interfaces
{
    public interface IBookService : ICrud<BookDTO>
    {
        IEnumerable<BookDTO> GetByFilter(FilterSearchDTO filterSearch, bool trackChanges);
    }
}
