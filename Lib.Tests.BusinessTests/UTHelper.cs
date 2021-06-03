using AutoMapper;
using Lib.Domain.Core.Models;
using Lib.Infrastructure.Business;
using Lib.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lib.Tests.BusinessTests
{
    internal static class UTHelper
    {
        public static DbContextOptions<LibDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<LibDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibDbContext(options))
            {
                SeedData(context);
            }
            return options;
        }

        public static void SeedData(LibDbContext context)
        {
            context.Books.Add(new Book { Id = 1, Author = "Stephen King", Title = "Pet Sematary", Year = 1983 });
            context.Books.Add(new Book { Id = 2, Author = "Chuck Palahniuk", Title = "Lullaby", Year = 2002 });
            context.Cards.Add(new Card { Id = 1, ReaderId = 1, Created = new DateTime(2020, 5, 23) });
            context.Readers.Add(new Reader { Id = 1, Name = "John Black", Email = "john_black@mail.ua" });
            context.Readers.Add(new Reader { Id = 2, Name = "Alice Petersen", Email = "alice123@gmail.com" });
            context.ReaderProfiles.Add(new ReaderProfile { ReaderId = 1, Phone = "12345", Address = "somewhere" });
            context.ReaderProfiles.Add(new ReaderProfile { ReaderId = 2, Phone = "54321",  Address = "far far away" });
            context.Histories.Add(new History { BookId = 1, CardId = 1, Id = 1, TakeDate = new DateTime(2021, 5, 22), ReturnDate = new DateTime(2021, 6, 1) });

            context.Cards.Add(new Card { Id = 2, ReaderId = 2, Created = new DateTime(2020, 7, 15) });
            context.Histories.Add(new History { Id = 2, BookId = 2, CardId = 2, TakeDate = new DateTime(2021, 5, 19) });
            context.SaveChanges();
        }

        public static Mapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}
