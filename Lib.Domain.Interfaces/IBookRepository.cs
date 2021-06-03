using Lib.Domain.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IQueryable<Book> FindAllWithDetails(bool trackChanges);

        Task<Book> GetByIdWithDetailsAsync(int id, bool trackChanges);
    }
}
