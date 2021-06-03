using Lib.Domain.Core.DTOs;
using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Lib.Infrastructure.Business.Services;
using Lib.Infrastructure.Business.Validation;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Tests.BusinessTests.ServiceTests
{
    [TestFixture]
    public class BooksServiceTests
    {
        [Test]
        public void BookService_FindAll_ReturnsBookModels()
        {
            var expected = GetTestBookDTOs().ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.BookRepository.FindAll(false))
                .Returns(GetTestBookEntities().AsQueryable);
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            var actual = bookService.GetAll(false).ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Author, actual[i].Author);
                Assert.AreEqual(expected[i].Title, actual[i].Title);
            }
        }

        private IEnumerable<BookDTO> GetTestBookDTOs()
        {
            return new List<BookDTO>()
            {
                new BookDTO {Id = 1, Author = "Isaac Asimov", Title = "Liar", Year = 1941},
                new BookDTO { Id = 2, Author = "Chuck Palahniuk", Title = "Lullaby", Year = 2002 },
                new BookDTO {Id = 3, Author = "Orson Scott Card", Title = "Ender’s Game", Year = 1985},
                new BookDTO {Id = 4, Author = "George Martin", Title = "A Song of Ice and Fire", Year = 1996},
                new BookDTO {Id = 5, Author = "Chuck Palahniuk", Title = "Choke", Year = 2001},
                new BookDTO {Id = 6, Author = "Orson Scott Card", Title = "Children of the Mind", Year = 1996}
            };
        }

        [Test]
        public async Task BookService_GetByIdAsync_ReturnsBookDTO()
        {
            var expected = GetTestBookDTOs().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.GetByIdWithDetailsAsync(It.IsAny<int>(), false))
                .ReturnsAsync(GetTestBookEntities().First);
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            var actual = await bookService.GetByIdAsync(1, false);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Author, actual.Author);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        private List<Book> GetTestBookEntities()
        {
            return new List<Book>()
            {
                new Book {Id = 1, Author = "Isaac Asimov", Title = "Liar", Year = 1941},
                new Book { Id = 2, Author = "Chuck Palahniuk", Title = "Lullaby", Year = 2002},
                new Book {Id = 3, Author = "Orson Scott Card", Title = "Ender’s Game", Year = 1985},
                new Book {Id = 4, Author = "George Martin", Title = "A Song of Ice and Fire", Year = 1996},
                new Book {Id = 5, Author = "Chuck Palahniuk", Title = "Choke", Year = 2001},
                new Book {Id = 6, Author = "Orson Scott Card", Title = "Children of the Mind", Year = 1996}
            };
        }

        [Test]
        public async Task BookService_AddAsync_AddsModel()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var book = new BookDTO { Id = 100, Author = "Stephen King", Title = "The Dead Zone" };

            //Act
            await bookService.AddAsync(book);

            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.AddAsync(It.Is<Book>(b => b.Author == book.Author && b.Id == book.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void BookService_AddAsync_ThrowsLibraryExceptionWithEmptyTitle()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var book = new BookDTO { Id = 100, Author = "Stephen King", Title = "" };

            Assert.ThrowsAsync<LibraryException>(() => bookService.AddAsync(book));
        }

        [Test]
        public void BookService_AddAsync_ThrowsLibraryExceptionWithEmptyAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var book = new BookDTO { Id = 100, Author = "", Title = "The Dead Zone" };

            Assert.ThrowsAsync<LibraryException>(() => bookService.AddAsync(book));
        }

        [Test]
        public void BookService_AddAsync_ThrowsLibraryExceptionWithInvalidYear()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var book = new BookDTO { Id = 100, Author = "Stephen King", Title = "The Dead Zone", Year = 9999 };

            Assert.ThrowsAsync<LibraryException>(() => bookService.AddAsync(book));
        }


        [Test]
        public async Task BookService_UpdateAsync_UpdatesBook()
        {
            //Arrange
            var book = new BookDTO { Id = 1, Author = "Stephen King", Title = "Firestarter" };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            //Act
            await bookService.UpdateAsync(book);

            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.Update(It.Is<Book>(b => b.Author == book.Author && b.Id == book.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void BookService_UpdateAsync_ThrowsLibraryExceptionWithEmptyAuthor()
        {
            //Arrange
            var book = new BookDTO { Id = 1, Author = "", Title = "Firestarter", Year = 1980 };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            Assert.ThrowsAsync<LibraryException>(() => bookService.UpdateAsync(book));
        }

        [Test]
        public void BookService_UpdateAsync_ThrowsLibraryExceptionWithEmptyTitle()
        {
            //Arrange
            var book = new BookDTO { Id = 1, Author = "Stephen King", Title = "", Year = 1980 };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            Assert.ThrowsAsync<LibraryException>(() => bookService.UpdateAsync(book));
        }

        [Test]
        public void BookService_UpdateAsync_ThrowsLibraryExceptionWithInvalidYear()
        {
            //Arrange
            var book = new BookDTO { Id = 1, Author = "Stephen King", Title = "Firestarter", Year = 9999 };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            Assert.ThrowsAsync<LibraryException>(() => bookService.UpdateAsync(book));
        }

        [Test]
        public void BookService_GetByFilter_ReturnsBooksByAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.FindAllWithDetails(false)).Returns(GetTestBookEntities().AsQueryable);
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var filter = new FilterSearchDTO { Author = "Chuck Palahniuk" };


            //Act
            var filteredBooks = bookService.GetByFilter(filter, false).ToList();

            Assert.AreEqual(2, filteredBooks.Count);
            foreach (var book in filteredBooks)
            {
                Assert.AreEqual(filter.Author, book.Author);
            }
        }

        [Test]
        public void BookService_GetByFilter_ReturnsBooksByYear()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.FindAllWithDetails(false)).Returns(GetTestBookEntities().AsQueryable);
            var bookService = new BookService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var filter = new FilterSearchDTO { Year = 1996 };

            var filteredBooks = bookService.GetByFilter(filter, false).ToList();

            Assert.AreEqual(2, filteredBooks.Count);
            foreach (var book in filteredBooks)
            {
                Assert.AreEqual(filter.Year, book.Year);
            }
        }
    }
}
