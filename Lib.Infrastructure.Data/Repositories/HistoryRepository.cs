using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Data.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly LibDbContext context;
        readonly DbSet<History> histories;

        public HistoryRepository(LibDbContext context)
        {
            this.context = context;
            histories = context.Histories;
        }

        public async Task AddAsync(History entity)
        {
            await histories.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(History entity)
        {
            histories.Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<History> FindAll(bool trackChanges) =>
            !trackChanges ? histories.AsNoTracking() : histories;

        public IQueryable<History> FindByCondition(Expression<Func<History, bool>> expression, bool trackChanges) =>
            !trackChanges ? histories.Where(expression).AsNoTracking() : histories.Where(expression);

        public IQueryable<History> GetAllWithDetails(bool trackChanges) =>
            FindAll(trackChanges)
            .Include(h => h.Card)
            .Include(h => h.Book);


        public void Update(History entity)
        {
            histories.Update(entity);
            context.SaveChanges();
        }
    }
}
