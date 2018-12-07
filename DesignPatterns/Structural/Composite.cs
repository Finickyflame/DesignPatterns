using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Composite : DesignPattern
    {
        public override void Execute()
        {
            IMovie firstMovie = new Movie("Slow and Safe");
            Assert.Equal("Slow and Safe", firstMovie.GetTitle());

            IMovie secondMovie = new Movie("The Beginnator");
            Assert.Equal("The Beginnator", secondMovie.GetTitle());

            var movieBundle = new MovieBundle(firstMovie);
            Assert.Equal("Bundle of: Slow and Safe", movieBundle.GetTitle());

            movieBundle.AddBook(secondMovie);
            Assert.Equal("Bundle of: Slow and Safe, The Beginnator", movieBundle.GetTitle());
        }

        #region Definition

        private interface IMovie
        {
            string GetTitle();
        }

        #endregion

        #region Concrete Implementation

        private class Movie : IMovie
        {
            private readonly string _title;

            public Movie(string title)
            {
                this._title = title;
            }

            public string GetTitle() => this._title;
        }

        private class MovieBundle : IMovie
        {
            private readonly List<IMovie> _books;

            public MovieBundle(params IMovie[] movies)
            {
                this._books = new List<IMovie>(movies);
            }

            public void AddBook(params IMovie[] movies)
            {
                this._books.AddRange(movies);
            }

            public string GetTitle()
            {
                return "Bundle of: " + string.Join(", ", this._books.Select(b => b.GetTitle()));
            }
        }

        #endregion
    }
}
