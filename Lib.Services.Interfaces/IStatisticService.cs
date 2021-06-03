using Lib.Domain.Core.DTOs;
using System;
using System.Collections.Generic;

namespace Lib.Services.Interfaces
{
    public interface IStatisticService
    {
        IEnumerable<BookDTO> GetMostPopularBooks(int bookCount, bool trackChanges);

        IEnumerable<ReaderActivityDTO> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate, bool trackChanges);
    }
}
