using Lib.Domain.Interfaces;
using Lib.Infrastructure.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly LibDbContext context;
        readonly IBookRepository bookRepository = null;
        readonly ICardRepository cardRepository = null;
        readonly IHistoryRepository historyRepository = null;
        readonly IReaderRepository readerRepository = null;

        public UnitOfWork()
        {
            context = new LibDbContext();
        }

        public IBookRepository BookRepository => bookRepository ?? new BookRepository(context);

        public ICardRepository CardRepository => cardRepository ?? new CardRepository(context);

        public IHistoryRepository HistoryRepository => historyRepository ?? new HistoryRepository(context);

        public IReaderRepository ReaderRepository => readerRepository ?? new ReaderRepository(context);

        public async Task<int> SaveAsync() => await context.SaveChangesAsync();

        #region IDisposable implementation

        bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    context.Dispose();

                disposedValue = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
