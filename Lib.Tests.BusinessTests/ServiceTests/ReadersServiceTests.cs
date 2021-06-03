using Lib.Domain.Core.DTOs;
using Lib.Domain.Core.Models;
using Lib.Domain.Interfaces;
using Lib.Infrastructure.Business.Services;
using Lib.Infrastructure.Business.Validation;
using Lib.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Tests.BusinessTests.ServiceTests
{
    [TestFixture]
    public class ReadersServiceTests
    {
        private DbContextOptions<LibDbContext> options;
        private readonly ReaderModelEqualityComparer readerModelComparer = new ReaderModelEqualityComparer();

        [SetUp]
        public void Setup()
        {
            options = UTHelper.GetUnitTestDbOptions();
        }

        [Test]
        public void ReaderService_GetAll_ReturnsReaderDTOs()
        {
            //arrange
            var expected = GetTestReaderDTOs().ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetAllWithDetails(false))
                .Returns(GetTestReaderEntities().AsQueryable());
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            //act
            var actual = readerService.GetAll(false).ToList();

            //assert
            Assert.IsInstanceOf<IEnumerable<ReaderDTO>>(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedReaderModel = expected[i];
                var actualReaderModel = actual[i];
                Assert.IsTrue(readerModelComparer.Equals(expectedReaderModel, actualReaderModel));
            }
        }

        [Test]
        public async Task ReaderService_GetByIdAsync_ReturnsReaderModels()
        {
            //arrange
            var expected = GetTestReaderDTOs().First();
            var id = 1;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetByIdWithDetailsAsync(It.IsAny<int>(), false))
                .ReturnsAsync(GetTestReaderEntities().First());
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            //act
            var actual = await readerService.GetByIdAsync(id, false);

            //assert
            Assert.IsInstanceOf<ReaderDTO>(actual);
            Assert.IsTrue(readerModelComparer.Equals(expected, actual));
        }

        [Test]
        public async Task ReaderService_AddAsync_AddModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.AddAsync(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());
            var newReader = new ReaderDTO()
            {
                Id = 10,
                Name = "Test_Adding",
                Email = "test_adding_email@gmail.com",
                Phone = "123789456",
                Address = "Kyiv, 10"
            };

            //act
            await readerService.AddAsync(newReader);

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.AddAsync(
                It.Is<Reader>(r => r.Id == newReader.Id && r.Name == newReader.Name
                && r.Email == newReader.Email && r.ReaderProfile.Phone == newReader.Phone
                && r.ReaderProfile.Address == newReader.Address)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task ReaderService_UpdateAsync_UpdateModel()
        {
            //arrange
            var reader = new ReaderDTO()
            {
                Id = 10,
                Name = "Test_Updating",
                Email = "test_adding_email@gmail.com",
                Phone = "159487263",
                Address = "test address"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.Update(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            //act
            await readerService.UpdateAsync(reader);

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.Update(
                It.Is<Reader>(r => r.Id == reader.Id && r.Name == reader.Name && r.Email == reader.Email)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void ReaderService_GetReadersThatDontReturnBooks_ReaderModels()
        {
            //arrange
            var expected = GetTestReaderDTOs().ToList();
            expected.RemoveAt(expected.Count - 1);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetAllWithDetails(false))
                .Returns(GetTestReaderEntities().AsQueryable());
            mockUnitOfWork
                .Setup(m => m.HistoryRepository.FindAll(false))
                .Returns(GetTestHistoryEntities().AsQueryable());
            mockUnitOfWork
                .Setup(m => m.CardRepository.FindAll(false))
                .Returns(GetTestCardEntities().AsQueryable());
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            //act
            var actual = readerService.GetReadersThatDontReturnBooks(false).ToList();

            //assert
            Assert.IsInstanceOf<IEnumerable<ReaderDTO>>(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedReaderModel = expected[i];
                var actualReaderModel = actual[i];
                Assert.IsTrue(readerModelComparer.Equals(expectedReaderModel, actualReaderModel));
            }
        }

        [Test]
        public void ReaderService_AddAsync_ThrowsExceptionIfModelIsIncorrect()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.AddAsync(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            // Name is empty
            var reader = new ReaderDTO
            {
                Name = "",
                Email = "only_money@gmail.com",
                Phone = "999999999",
                Address = "Glasgow"
            };
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.AddAsync(reader));

            // Email is empty
            reader.Name = "Scrooge McDuck";
            reader.Email = "";
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.AddAsync(reader));

            // Phone is empty
            reader.Email = "only_money@gmail.com";
            reader.Phone = "";
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.AddAsync(reader));

            // Address is empty
            reader.Phone = "999999999";
            reader.Address = "";
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.AddAsync(reader));
        }

        [Test]
        public void ReaderService_UpdateAsync_ThrowsExceptionIfModelIsIncorrect()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.Update(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UTHelper.CreateMapperProfile());

            // Name is empty
            var reader = new ReaderDTO
            {
                Id = 1,
                Name = "",
                Email = "only_money@gmail.com",
                Phone = "999999999",
                Address = "Glasgow"
            };
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.UpdateAsync(reader));

            // Email is empty
            reader.Name = "Scrooge McDuck";
            reader.Email = "";
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.UpdateAsync(reader));

            // Phone is empty
            reader.Email = "only_money@gmail.com";
            reader.Phone = "";
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.UpdateAsync(reader));

            // Address is empty
            reader.Phone = "999999999";
            reader.Address = "";
            Assert.ThrowsAsync<LibraryException>(async () => await readerService.UpdateAsync(reader));
        }

        #region data for tests
        private IEnumerable<ReaderDTO> GetTestReaderDTOs()
        {
            return new List<ReaderDTO>()
            {
                new ReaderDTO { Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    Phone = "123456789", Address = "Kyiv, 1" },
                new ReaderDTO { Id = 2, Name = "Ivan", Email = "ivan_email@gmail.com",
                    Phone = "456789123", Address = "Kyiv, 2" },
                new ReaderDTO { Id = 3, Name = "Petro", Email = "petro_email@gmail.com",
                    Phone = "789123456", Address = "Kyiv, 3" },
                new ReaderDTO { Id = 4, Name = "Oleksandr", Email = "oleksandr_email@gmail.com",
                    Phone = "326159487", Address = "Kyiv, 4" }
            };
        }

        private IEnumerable<Reader> GetTestReaderEntities()
        {
            return new List<Reader>()
            {
                 new Reader { Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 1, Phone = "123456789", Address = "Kyiv, 1" }},
                 new Reader { Id = 2, Name = "Ivan", Email = "ivan_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 2, Phone = "456789123", Address = "Kyiv, 2" }},
                 new Reader { Id = 3, Name = "Petro", Email = "petro_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 3, Phone = "789123456", Address = "Kyiv, 3" }},
                 new Reader { Id = 4, Name = "Oleksandr", Email = "oleksandr_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 4, Phone = "326159487", Address = "Kyiv, 4" }}
            };
        }

        private IEnumerable<Card> GetTestCardEntities()
        {
            return new List<Card>()
            {
                new Card { Id = 1, ReaderId = 1, Created = DateTime.Now.AddDays(-10) },
                new Card { Id = 2, ReaderId = 2, Created = DateTime.Now.AddDays(-7) },
                new Card { Id = 3, ReaderId = 3, Created = DateTime.Now.AddDays(-3) },
                new Card { Id = 4, ReaderId = 4, Created = DateTime.Now },
            };
        }
        private IEnumerable<History> GetTestHistoryEntities()
        {
            return new List<History>()
            {
                new History { Id = 1, BookId = 1, CardId = 1, TakeDate = DateTime.Now.AddDays(-10), ReturnDate = DateTime.Now.AddDays(-7) },
                new History { Id = 2, BookId = 2, CardId = 2, TakeDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now.AddDays(-1) },
                new History { Id = 3, BookId = 3, CardId = 2, TakeDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now.AddDays(-4) },
                new History { Id = 4, BookId = 2, CardId = 3, TakeDate = DateTime.Now.AddDays(-3), ReturnDate = DateTime.Now },
                new History { Id = 5, BookId = 1, CardId = 2, TakeDate = DateTime.Now.AddDays(-7) },
                new History { Id = 6, BookId = 3, CardId = 1, TakeDate = DateTime.Now.AddDays(-3) },
                new History { Id = 7, BookId = 4, CardId = 3, TakeDate = DateTime.Now.AddDays(-3) }
            };
        }
        #endregion
    }
}
