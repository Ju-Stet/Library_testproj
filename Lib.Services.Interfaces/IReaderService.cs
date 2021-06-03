using Lib.Domain.Core.DTOs;
using System.Collections.Generic;

namespace Lib.Services.Interfaces
{
    public interface IReaderService : ICrud<ReaderDTO>
    {
        IEnumerable<ReaderDTO> GetReadersThatDontReturnBooks(bool trackChanges);
    }
}
