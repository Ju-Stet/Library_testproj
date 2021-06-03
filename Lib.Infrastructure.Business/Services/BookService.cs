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
    public class BookService : IBookService
    {
        readonly IUnitOfWork context;
        readonly Mapper mapper;
        readonly IBookRepository repository;

        public BookService(IUnitOfWork context, Mapper mapper = null)
        {
            this.context = context;
            repository = context.BookRepository;
            if (mapper == null)
            {
                var profile = new AutomapperProfile();
                var config = new MapperConfiguration(c => c.AddProfile(profile));

                this.mapper = new Mapper(config);
            }
            else
                this.mapper = mapper;
        }

        public async Task AddAsync(BookDTO model)
        {
            Validate(model);

            await repository.AddAsync(mapper.Map<Book>(model));
            await context.SaveAsync();
        }



        public async Task DeleteByIdAsync(int modelId)
        {
            var book = repository.FindByCondition(x => x.Id == modelId, false).FirstOrDefault();
            repository.Delete(book);

            await context.SaveAsync();
        }


        public IEnumerable<BookDTO> GetAll(bool trackChanges)
        {
            List<BookDTO> bookModels = new List<BookDTO>();
            var books = repository.FindAll(trackChanges);

            foreach (var item in books)
            {
                bookModels.Add(mapper.Map<BookDTO>(item));
            }

            return bookModels;
        }

        public IEnumerable<BookDTO> GetByFilter(FilterSearchDTO filterSearch, bool trackChanges)
        {
            List<BookDTO> bookModels = new List<BookDTO>();

            var books = repository.FindAllWithDetails(trackChanges)
                .Where(b => (b.Author == filterSearch.Author)
                            || (b.Year == filterSearch.Year));


            foreach (var item in books)
            {
                bookModels.Add(mapper.Map<BookDTO>(item));
            }

            return bookModels;
        }

        public async Task<BookDTO> GetByIdAsync(int id, bool trackChanges)
            => mapper.Map<BookDTO>(await repository.GetByIdWithDetailsAsync(id, trackChanges));


        public async Task UpdateAsync(BookDTO model)
        {
            Validate(model);

            repository.Update(mapper.Map<Book>(model));
            await context.SaveAsync();
        }

        void Validate(BookDTO model)
        {
            if (string.IsNullOrEmpty(model.Author))
                throw new LibraryException("Author shoud not be empty");
            if (string.IsNullOrEmpty(model.Title))
                throw new LibraryException("Title shoud not be empty");
            if (model.Year > DateTime.Now.Year)
                throw new LibraryException("Year shoud not be in the future");
        }
    }
}
