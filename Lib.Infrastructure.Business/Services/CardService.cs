using AutoMapper;
using Lib.Domain.Core.DTOs;
using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Lib.Infrastructure.Business.Validation;
using Lib.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Infrastructure.Business.Services
{
    public class CardService : ICardService
    {
        readonly IUnitOfWork context;
        readonly Mapper mapper;
        readonly ICardRepository repository;

        public CardService(IUnitOfWork context, Mapper mapper = null)
        {
            this.context = context;
            repository = context.CardRepository;
            if (mapper == null)
            {
                var profile = new AutomapperProfile();
                var config = new MapperConfiguration(c => c.AddProfile(profile));

                this.mapper = new Mapper(config);
            }
            else
                this.mapper = mapper;
        }

        public async Task AddAsync(CardDTO model)
        {
            Validate(model);

            await repository.AddAsync(mapper.Map<Card>(model));
            await context.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            var card = repository.FindByCondition(x => x.Id == modelId, false).FirstOrDefault();
            repository.Delete(card);

            await context.SaveAsync();
        }

        public IEnumerable<CardDTO> GetAll(bool trackChanges)
        {
            List<CardDTO> cardModels = new List<CardDTO>();
            var cards = repository.FindAllWithDetails(trackChanges);

            foreach (var item in cards)
            {
                cardModels.Add(mapper.Map<CardDTO>(item));
            }

            return cardModels;
        }

        public IEnumerable<BookDTO> GetBooksByCardId(int cardId, bool trackChanges)
        {
            List<BookDTO> bookModels = new List<BookDTO>();

            var books = repository.GetByIdWithDetailsAsync(cardId, trackChanges).Result
                .Books.Select(x => x.Book);

            foreach (var item in books)
            {
                bookModels.Add(mapper.Map<BookDTO>(item));
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookDTO>> GetBooksByCardIdAsync(int cardId, bool trackChanges)
        {
            List<BookDTO> bookModels = new List<BookDTO>();

            var card = await repository.GetByIdWithDetailsAsync(cardId, trackChanges);

            var books = card.Books.Select(x => x.Book);

            foreach (var item in books)
            {
                bookModels.Add(mapper.Map<BookDTO>(item));
            }

            return bookModels;
        }

        public async Task<CardDTO> GetByIdAsync(int id, bool trackChanges)
         => mapper.Map<CardDTO>(await context.CardRepository.GetByIdWithDetailsAsync(id, trackChanges));

        public async Task HandOverBookAsync(int cartId, int bookId)
        {
            var histories = context.HistoryRepository.GetAllWithDetails(false);

            if (!histories.Any(x => x.CardId == cartId))
                throw new LibraryException($"There is no record with card id {cartId}");

            var record = histories
                .Where(x => x.CardId == cartId && x.BookId == bookId)
                .FirstOrDefault();

            if (record.ReturnDate != null)
                throw new LibraryException("The book is aldeary returned");

            record.ReturnDate = DateTime.Now.Date;
            await Task.Run(() => context.HistoryRepository.Update(record));
            await context.SaveAsync();
        }

        public async Task TakeBookAsync(int cartId, int bookId)
        {
            var card = await context.CardRepository.GetByIdWithDetailsAsync(cartId, false);
            if (card == null)
                throw new LibraryException($"There is no card with id {cartId}");

            var book = await context.BookRepository.GetByIdWithDetailsAsync(bookId, false);
            if (book == null)
                throw new LibraryException($"There is no book with id {bookId}");

            var record = context.HistoryRepository
                .GetAllWithDetails(false)
                .Where(x => x.CardId == cartId && x.BookId == bookId)
                .FirstOrDefault();
            if (record != null && record.ReturnDate == null)
                throw new LibraryException("The book is aldeary taken");

            History history = new History
            {
                Id = context.HistoryRepository.FindAll(false).Count() + 1,
                BookId = bookId,
                CardId = cartId,
                TakeDate = DateTime.Now.Date,
                Book = context.BookRepository.FindByCondition(b => b.Id == bookId, false).FirstOrDefault(),
                Card = context.CardRepository.FindByCondition(c => c.Id == cartId, false).FirstOrDefault()
            };

            await context.HistoryRepository.AddAsync(history);
            await context.SaveAsync();
        }

        public async Task UpdateAsync(CardDTO model)
        {
            Validate(model);

            await Task.Run(() => context.CardRepository.Update(mapper.Map<Card>(model)));
            await context.SaveAsync();
        }

        void Validate(CardDTO model)
        {
            if (model == null)
                throw new LibraryException("CardModel shoud not be null");
            if (model.Created > DateTime.Now.Date)
                throw new LibraryException("Date shoud not be in the future");
        }
    }
}
