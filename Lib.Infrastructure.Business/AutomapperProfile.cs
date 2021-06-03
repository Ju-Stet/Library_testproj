using AutoMapper;
using Lib.Domain.Core.DTOs;
using Lib.Domain.Core.Models;
using System.Linq;

namespace Lib.Infrastructure.Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(p => p.CardsIds, c => c.MapFrom(book => book.Cards.Select(x => x.CardId)))
                .ReverseMap();

            CreateMap<Card, CardDTO>()
                .ForMember(p => p.BooksIds, c => c.MapFrom(card => card.Books.Select(x => x.BookId)))
                .ReverseMap();

            CreateMap<Reader, ReaderDTO>()
                .IncludeMembers(p => p.ReaderProfile)
                .ReverseMap();

            CreateMap<ReaderProfile, ReaderDTO>()
                .ReverseMap();

        }
    }
}
