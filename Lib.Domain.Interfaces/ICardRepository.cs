using Lib.Domain.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Domain.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card> GetByIdWithDetailsAsync(int id, bool trackChanges);
        IQueryable<Card> FindAllWithDetails(bool trackChanges);
    }
}
