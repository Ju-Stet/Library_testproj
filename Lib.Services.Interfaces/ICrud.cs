using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.Services.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll(bool trackChanges);

        Task<TModel> GetByIdAsync(int id, bool trackChanges);

        Task AddAsync(TModel model);

        Task UpdateAsync(TModel model);

        Task DeleteByIdAsync(int modelId);
    }
}
