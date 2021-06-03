using Lib.Domain.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Domain.Interfaces
{
    public interface IReaderRepository : IRepository<Reader>
    {
        IQueryable<Reader> GetAllWithDetails(bool trackChanges);
        Task<Reader> GetByIdWithDetailsAsync(int id, bool trackChanges);
    }
}
