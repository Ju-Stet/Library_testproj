using System;
using System.Threading.Tasks;

namespace Lib.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }

        ICardRepository CardRepository { get; }

        IHistoryRepository HistoryRepository { get; }

        IReaderRepository ReaderRepository { get; }

        Task<int> SaveAsync();
    }
}
