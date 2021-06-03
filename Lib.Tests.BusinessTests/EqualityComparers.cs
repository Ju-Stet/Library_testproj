using Lib.Domain.Core.DTOs;
using Lib.Domain.Core.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lib.Tests.BusinessTests
{
    internal class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals([AllowNull] Card x, [AllowNull] Card y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] Card obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class CardModelEqualityComparer : IEqualityComparer<CardDTO>
    {
        public bool Equals([AllowNull] CardDTO x, [AllowNull] CardDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] CardDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class HistoryEqualityComparer : IEqualityComparer<History>
    {
        public bool Equals([AllowNull] History x, [AllowNull] History y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.BookId == y.BookId
                && x.CardId == y.CardId
                && x.TakeDate == y.TakeDate
                && x.ReturnDate == y.ReturnDate;
        }

        public int GetHashCode([DisallowNull] History obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals([AllowNull] Book x, [AllowNull] Book y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] Book obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookModelEqualityComparer : IEqualityComparer<BookDTO>
    {
        public bool Equals([AllowNull] BookDTO x, [AllowNull] BookDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] BookDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReaderModelEqualityComparer : IEqualityComparer<ReaderDTO>
    {
        public bool Equals([AllowNull] ReaderDTO x, [AllowNull] ReaderDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && string.Equals(x.Name, y.Name)
                && string.Equals(x.Email, y.Email)
                && string.Equals(x.Phone, y.Phone)
                && string.Equals(x.Address, y.Address);
        }

        public int GetHashCode([DisallowNull] ReaderDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReaderEqualityComparer : IEqualityComparer<Reader>
    {
        public bool Equals([AllowNull] Reader x, [AllowNull] Reader y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && string.Equals(x.Name, y.Name)
                   && string.Equals(x.Email, y.Email);
        }

        public int GetHashCode([DisallowNull] Reader obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReaderActivityModelEqualityComparer : IEqualityComparer<ReaderActivityDTO>
    {
        public bool Equals([AllowNull] ReaderActivityDTO x, [AllowNull] ReaderActivityDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.ReaderId == y.ReaderId &&
                   x.BooksCount == y.BooksCount &&
                   string.Equals(x.ReaderName, y.ReaderName);
        }

        public int GetHashCode([DisallowNull] ReaderActivityDTO obj)
        {
            return obj.GetHashCode();
        }
    }
}
