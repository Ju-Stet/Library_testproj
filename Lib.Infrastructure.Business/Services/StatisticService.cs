using AutoMapper;
using Lib.Domain.Core.DTOs;
using Lib.Domain.Interfaces;
using Lib.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Infrastructure.Business.Services
{
    public class StatisticService : IStatisticService
    {
        readonly IUnitOfWork context;
        readonly Mapper mapper;

        public StatisticService(IUnitOfWork context, Mapper mapper = null)
        {
            this.context = context;
            if (mapper == null)
            {
                var profile = new AutomapperProfile();
                var config = new MapperConfiguration(c => c.AddProfile(profile));

                this.mapper = new Mapper(config);
            }
            else
                this.mapper = mapper;
        }

        public IEnumerable<BookDTO> GetMostPopularBooks(int bookCount, bool trackChanges)
        {
            List<BookDTO> bookModels = new List<BookDTO>();

            var histories = context.HistoryRepository.GetAllWithDetails(trackChanges);

            var query = from history in histories
                        group history by history.BookId into grp
                        let count = grp.Count()
                        orderby count descending
                        select new { Count = count, Value = grp };

            for (int i = 0; i < bookCount; i++)
            {
                var item = query.ElementAtOrDefault(i).Value
                    .Select(x => x.Book).FirstOrDefault();

                bookModels.Add(mapper.Map<BookDTO>(item));
            }

            return bookModels;
        }

        public IEnumerable<ReaderActivityDTO> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate, bool trackChanges)
        {

            List<ReaderActivityDTO> readerActivities = new List<ReaderActivityDTO>();

            var histories = context.HistoryRepository.GetAllWithDetails(trackChanges);

            var query = from history in histories
                        where history.TakeDate >= firstDate && history.ReturnDate <= lastDate
                        group history by history.Card.ReaderId into grp
                        let count = grp.Count()
                        orderby count descending
                        select new { Count = count, Value = grp };

            for (int i = 0; i < readersCount; i++)
            {
                var item = query.ElementAtOrDefault(i).Value
                    .Select(x => x.Card.Reader).FirstOrDefault();

                readerActivities.Add(new ReaderActivityDTO
                {
                    BooksCount = query.ElementAtOrDefault(i).Count,
                    ReaderId = item.Id,
                    ReaderName = item.Name
                });
            }

            return readerActivities;
        }
    }
}
