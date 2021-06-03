using Lib.Domain.Core.Models;
using System.Linq;

namespace Lib.Domain.Interfaces
{
    public interface IHistoryRepository : IRepository<History>
    {
        IQueryable<History> GetAllWithDetails(bool trackChanges);
    }
}
