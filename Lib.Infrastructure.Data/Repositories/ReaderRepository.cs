using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Data.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        readonly LibDbContext context;
        readonly DbSet<Reader> readers;

        public ReaderRepository(LibDbContext context)
        {
            this.context = context;
            readers = context.Readers;
        }

        public async Task AddAsync(Reader entity)
        {
            await readers.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Reader entity)
        {
            readers.Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<Reader> FindAll(bool trackChanges) =>
            !trackChanges ? readers.AsNoTracking() : readers;

        public IQueryable<Reader> FindByCondition(Expression<Func<Reader, bool>> expression, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Reader> GetAllWithDetails(bool trackChanges) =>
            FindAll(trackChanges)
            .Include(r => r.ReaderProfile)
            .Include(r => r.Cards);

        public async Task<Reader> GetByIdWithDetailsAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.Id == id, trackChanges)
            .Include(r => r.ReaderProfile)
            .Include(r => r.Cards)
            .FirstOrDefaultAsync();

        public void Update(Reader entity)
        {
            readers.Update(entity);
            context.SaveChanges();
        }
    }
}
