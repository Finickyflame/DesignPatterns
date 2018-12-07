using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignPatterns.Creational
{
    public class ObjectPool : DesignPattern
    {
        public override void Execute()
        {
            var openingDate = new DateTime(2000, 1, 1);
            var library = new Library(openingDate, inventoryCount: 2);

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

        private class Book
        {
            public Book(DateTime purchasedOn)
            {
                this.PurchasedOn = purchasedOn;
            }

            public DateTime PurchasedOn { get; }
        }

        private class Library
        {
            private readonly Queue<Book> _inventory;

            public Library(DateTime openingDate, int inventoryCount, int minInventoryCount = 0)
            {
                this._inventory = new Queue<Book>(Enumerable.Repeat(new Book(openingDate), Math.Max(inventoryCount, minInventoryCount)));
                this.MinInventoryCount = minInventoryCount;
            }

            public int InventoryCount => this._inventory.Count;

            private int MinInventoryCount { get; }

            public Book RentBook()
            {
                return this.InventoryCount > this.MinInventoryCount ? this.GetBookFromInventory() : PurchaseBook();
            }

            public void ReturnBook(Book book)
            {
                this.AddBookToInventory(book);
            }

            private Book GetBookFromInventory()
            {
                return this._inventory.Dequeue();
            }

            private void AddBookToInventory(Book book)
            {
                this._inventory.Enqueue(book);
            }

            private static Book PurchaseBook()
            {
                return new Book(DateTime.UtcNow);
            }
        }
    }
}
