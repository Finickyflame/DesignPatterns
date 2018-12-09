using System;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Bridge : DesignPattern
    {
        public override void Execute()
        {
            const string authorName = "John Doe";
            const string bookName = "Best book in the world";

            Book authorAndName = new BookWithAuthorAndName(authorName, bookName, FontStyle.Normal);

            Assert.Equal("John Doe", authorAndName.AuthorName);
            Assert.Equal("Best book in the world", authorAndName.BookName);
            Assert.Equal("John Doe's Best book in the world", authorAndName.Title);

            Book authorAndNameUppercase = new BookWithAuthorAndName(authorName, bookName, FontStyle.Uppercase);

            Assert.Equal("JOHN DOE", authorAndNameUppercase.AuthorName);
            Assert.Equal("BEST BOOK IN THE WORLD", authorAndNameUppercase.BookName);
            Assert.Equal("JOHN DOE's BEST BOOK IN THE WORLD", authorAndNameUppercase.Title);


            Book nameAndAuthor = new BookWithNameAndAuthor(authorName, bookName, FontStyle.Normal);

            Assert.Equal("John Doe", nameAndAuthor.AuthorName);
            Assert.Equal("Best book in the world", nameAndAuthor.BookName);
            Assert.Equal("Best book in the world by John Doe", nameAndAuthor.Title);

            Book nameAndAuthorUppercase = new BookWithNameAndAuthor(authorName, bookName, FontStyle.Uppercase);

            Assert.Equal("JOHN DOE", nameAndAuthorUppercase.AuthorName);
            Assert.Equal("BEST BOOK IN THE WORLD", nameAndAuthorUppercase.BookName);
            Assert.Equal("BEST BOOK IN THE WORLD by JOHN DOE", nameAndAuthorUppercase.Title);
        }

        #region Definition

        public enum FontStyle
        {
            Uppercase,
            Normal
        }

        /* Bridge */
        public abstract class Book
        {
            private readonly string _authorName;
            private readonly string _bookName;
            private readonly BookStyle _bookStyle;

            protected Book(string authorName, string bookName, FontStyle titleStyle)
            {
                this._authorName = authorName;
                this._bookName = bookName;
                this._bookStyle = GetBookStyle(titleStyle);
            }

            public string AuthorName => this._bookStyle.StyleAuthorName(this._authorName);
            public string BookName => this._bookStyle.StyleBookName(this._bookName);

            public abstract string Title { get; }

            private static BookStyle GetBookStyle(FontStyle fontStyle)
            {
                switch (fontStyle)
                {
                    case FontStyle.Uppercase:
                        return new BookStyleUppercase();
                    case FontStyle.Normal:
                        return new BookStyleNormal();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fontStyle), fontStyle, null);
                }
            }
        }

        /* Bridge Implementation */
        public abstract class BookStyle
        {
            public abstract string StyleAuthorName(string authorName);
            public abstract string StyleBookName(string bookName);
        }

        #endregion

        #region Concrete Implementation

        public class BookWithAuthorAndName : Book
        {
            public BookWithAuthorAndName(string authorName, string bookName, FontStyle titleStyle)
                : base(authorName, bookName, titleStyle)
            {
            }

            public override string Title => $"{this.AuthorName}'s {this.BookName}";
        }

        public class BookWithNameAndAuthor : Book
        {
            public BookWithNameAndAuthor(string authorName, string bookName, FontStyle titleStyle)
                : base(authorName, bookName, titleStyle)
            {
            }

            public override string Title => $"{this.BookName} by {this.AuthorName}";
        }

        public class BookStyleUppercase : BookStyle
        {
            public override string StyleAuthorName(string authorName) => authorName?.ToUpper();

            public override string StyleBookName(string bookName) => bookName?.ToUpper();
        }

        public class BookStyleNormal : BookStyle
        {
            public override string StyleAuthorName(string authorName) => authorName;

            public override string StyleBookName(string bookName) => bookName;
        }
        #endregion
    }
}
