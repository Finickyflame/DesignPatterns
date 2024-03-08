namespace DesignPatterns.Creational;

public class ObjectPool
{
    [Fact]
    public void Execute()
    {
        var openingDate = new DateTime(2000, 1, 1);
        var library = new Library(openingDate, 2);

        Assert.Equal(2, library.InventoryCount);

        Book firstBook = library.RentBook();
        Assert.False(firstBook.PurchasedOn > openingDate);
        Assert.Equal(openingDate, firstBook.PurchasedOn);
        Assert.Equal(1, library.InventoryCount);

        Book secondBook = library.RentBook();
        Assert.False(secondBook.PurchasedOn > openingDate);
        Assert.Equal(openingDate, secondBook.PurchasedOn);
        Assert.Equal(0, library.InventoryCount);

        Book newBook = library.RentBook();
        Assert.True(newBook.PurchasedOn > openingDate);
        Assert.NotEqual(openingDate, newBook.PurchasedOn);
        Assert.Equal(0, library.InventoryCount);

        library.ReturnBook(firstBook);
        Assert.Equal(1, library.InventoryCount);

        library.ReturnBook(secondBook);
        Assert.Equal(2, library.InventoryCount);

        library.ReturnBook(newBook);
        Assert.Equal(3, library.InventoryCount);
    }

    public class Book(DateTime purchasedOn)
    {
        public DateTime PurchasedOn { get; } = purchasedOn;
    }

    public class Library(DateTime openingDate, int inventoryCount, int minInventoryCount = 0)
    {
        private readonly Queue<Book> _inventory = new(Enumerable.Repeat(new Book(openingDate), Math.Max(inventoryCount, minInventoryCount)));

        public int InventoryCount => _inventory.Count;

        private int MinInventoryCount { get; } = minInventoryCount;

        public Book RentBook()
        {
            if (InventoryCount > MinInventoryCount)
            {
                return GetBookFromInventory();
            }
            return PurchaseBook();
        }

        public void ReturnBook(Book book)
        {
            AddBookToInventory(book);
        }

        private Book GetBookFromInventory() => _inventory.Dequeue();

        private void AddBookToInventory(Book book)
        {
            _inventory.Enqueue(book);
        }

        private static Book PurchaseBook() => new(DateTime.UtcNow);
    }
}