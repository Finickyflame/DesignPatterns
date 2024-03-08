namespace DesignPatterns.Structural;

public class Bridge
{
    [Fact]
    public void Execute()
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

    public enum FontStyle
    {
        Uppercase,
        Normal
    }

    /* Bridge */
    public abstract class Book(string authorName, string bookName, FontStyle titleStyle)
    {
        private readonly BookStyle _bookStyle = GetBookStyle(titleStyle);

        public string AuthorName => _bookStyle.StyleAuthorName(authorName);
        public string BookName => _bookStyle.StyleBookName(bookName);

        public abstract string Title { get; }

        private static BookStyle GetBookStyle(FontStyle fontStyle) => fontStyle switch
        {
            FontStyle.Uppercase => new BookStyleUppercase(),
            FontStyle.Normal => new BookStyleNormal(),
            _ => throw new ArgumentOutOfRangeException(nameof(fontStyle), fontStyle, null)
        };
    }

    /* Bridge Implementation */
    public abstract class BookStyle
    {
        public abstract string StyleAuthorName(string authorName);
        public abstract string StyleBookName(string bookName);
    }

    public class BookWithAuthorAndName(string authorName, string bookName, FontStyle titleStyle) : Book(authorName, bookName, titleStyle)
    {
        public override string Title => $"{AuthorName}'s {BookName}";
    }

    public class BookWithNameAndAuthor(string authorName, string bookName, FontStyle titleStyle) : Book(authorName, bookName, titleStyle)
    {
        public override string Title => $"{BookName} by {AuthorName}";
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
}