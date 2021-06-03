using Lib.Domain.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.Services.Interfaces
{
    public interface ICardService : ICrud<CardDTO>
    {
        IEnumerable<BookDTO> GetBooksByCardId(int cardId, bool trackChanges);

        Task TakeBookAsync(int cartId, int bookId);

        Task HandOverBookAsync(int cartId, int bookId);
    }
}
