using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        readonly LibDbContext context;
        readonly DbSet<Book> books;

        public BookRepository(LibDbContext context)           
        {
            this.context = context;
            books = context.Books;
        }

        public async Task AddAsync(Book entity)
        {
            await books.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Book entity)
        {
            books.Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<Book> FindAll(bool trackChanges) =>
            !trackChanges ? books.AsNoTracking() : books;

        public IQueryable<Book> FindAllWithDetails(bool trackChanges) =>
            FindAll(trackChanges)
            .Include(b => b.Cards);

        public IQueryable<Book> FindByCondition(Expression<Func<Book, bool>> expression, bool trackChanges) =>
            !trackChanges ? books.Where(expression).AsNoTracking() : books.Where(expression);

        public async Task<Book> GetByIdWithDetailsAsync(int id, bool trackChanges) => 
            await FindByCondition(b => b.Id == id, trackChanges)
            .Include(b => b.Cards)
            .FirstOrDefaultAsync();

        public void Update(Book entity)
        {
            books.Update(entity);
            context.SaveChanges();
        }
    }
}
