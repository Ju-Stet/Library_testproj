using AutoMapper;
using Lib.Domain.Core.DTOs;
using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Lib.Infrastructure.Business.Validation;
using Lib.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Business.Services
{
    public class ReaderService : IReaderService
    {
        readonly IUnitOfWork context;
        readonly Mapper mapper;
        readonly IReaderRepository repository;

        public ReaderService(IUnitOfWork context, Mapper mapper = null)
        {
            this.context = context;
            repository = context.ReaderRepository;
            if (mapper == null)
            {
                var profile = new AutomapperProfile();
                var config = new MapperConfiguration(c => c.AddProfile(profile));

                this.mapper = new Mapper(config);
            }
            else
                this.mapper = mapper;
        }

        public async Task AddAsync(ReaderDTO model)
        {
            Validate(model);

            var reader = mapper.Map<Reader>(model);
            await repository.AddAsync(reader);
            await context.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            var reader = repository.FindByCondition(x => x.Id == modelId, false).FirstOrDefault();
            repository.Delete(reader);

            await context.SaveAsync();
        }

        public IEnumerable<ReaderDTO> GetAll(bool trackChanges)
        {
            List<ReaderDTO> readerModels = new List<ReaderDTO>();
            var books = repository.GetAllWithDetails(trackChanges);

            foreach (var item in books)
            {
                readerModels.Add(mapper.Map<ReaderDTO>(item));
            }
            var res = readerModels;
            return res;
        }

        public async Task<ReaderDTO> GetByIdAsync(int id, bool trackChanges)
            => mapper.Map<ReaderDTO>(await repository.GetByIdWithDetailsAsync(id, trackChanges));

        public IEnumerable<ReaderDTO> GetReadersThatDontReturnBooks(bool trackChanges)
        {
            List<ReaderDTO> readerModels = new List<ReaderDTO>();

            var readers = repository.GetAllWithDetails(trackChanges);
            var cards = context.CardRepository.FindAll(false);
            var histories = context.HistoryRepository.FindAll(false);
            var query =
                                    from reader in readers
                                    join card in cards on reader.Id equals card.ReaderId
                                    join history in histories on card.Id equals history.CardId
                                    where history.ReturnDate == null
                                    select new Reader
                                    {
                                        Id = reader.Id,
                                        Name = reader.Name,
                                        Email = reader.Email,
                                        ReaderProfile = reader.ReaderProfile,
                                        Cards = reader.Cards
                                    };

            foreach (var item in query)
            {
                readerModels.Add(mapper.Map<ReaderDTO>(item));
            }

            return readerModels;
        }

        public async Task UpdateAsync(ReaderDTO model)
        {
            Validate(model);

            context.ReaderRepository.Update(mapper.Map<Reader>(model));
            await context.SaveAsync();
        }

        void Validate(ReaderDTO model)
        {
            string mailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            if (string.IsNullOrEmpty(model.Name))
                throw new LibraryException("Name shoud not be empty");
            if (string.IsNullOrEmpty(model.Email))
                throw new LibraryException("Email shoud not be empty");
            if (Regex.Matches(model.Email, mailPattern).Count == 0)
                throw new LibraryException("Email shoud not be empty");
            if (string.IsNullOrEmpty(model.Phone))
                throw new LibraryException("Phone shoud not be empty");
            if (string.IsNullOrEmpty(model.Address))
                throw new LibraryException("Address shoud not be empty");
        }
    }
}
