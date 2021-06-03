using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        readonly LibDbContext context;
        readonly DbSet<Card> cards;

        public CardRepository(LibDbContext context)
        {
            this.context = context;
            cards = context.Cards;
        }

        public async Task AddAsync(Card entity)
        {
            await cards.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Card entity)
        {
            cards.Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<Card> FindAll(bool trackChanges) =>
            !trackChanges ? cards.AsNoTracking() : cards;

        public IQueryable<Card> FindAllWithDetails(bool trackChanges) =>
            FindAll(trackChanges)
            .Include(c => c.Books)
            .Include(c => c.Reader);

        public IQueryable<Card> FindByCondition(Expression<Func<Card, bool>> expression, bool trackChanges) =>
            !trackChanges ? cards.Where(expression).AsNoTracking() : cards.Where(expression);

        public async Task<Card> GetByIdWithDetailsAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.Id == id, trackChanges)
            .Include(c => c.Books)
            .Include(c => c.Reader)
            .FirstOrDefaultAsync(c => c.Id == id);

        public void Update(Card entity)
        {
            cards.Update(entity);
            context.SaveChanges();
        }        
    }
}
