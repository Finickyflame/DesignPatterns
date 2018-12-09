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

            movieBundle.AddMovie(secondMovie);
            Assert.Equal("Bundle of: Slow and Safe, The Beginnator", movieBundle.GetTitle());
        }

        #region Definition

        public interface IMovie
        {
            string GetTitle();
        }

        #endregion

        #region Concrete Implementation

        public class Movie : IMovie
        {
            private readonly string _title;

            public Movie(string title)
            {
                this._title = title;
            }

            public string GetTitle() => this._title;
        }

        public class MovieBundle : IMovie
        {
            private readonly List<IMovie> _movies;

            public MovieBundle(params IMovie[] movies)
            {
                this._movies = new List<IMovie>(movies);
            }

            public void AddMovie(params IMovie[] movies)
            {
                this._movies.AddRange(movies);
            }

            public string GetTitle()
            {
                return "Bundle of: " + string.Join(", ", this._movies.Select(b => b.GetTitle()));
            }
        }

        #endregion
    }
}
