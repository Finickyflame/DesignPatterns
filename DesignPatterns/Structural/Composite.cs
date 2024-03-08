namespace DesignPatterns.Structural;

public class Composite
{
    [Fact]
    public void Execute()
    {
        IMovie firstMovie = new Movie("Slow and Safe");
        Assert.Equal("Slow and Safe", firstMovie.Title);

        IMovie secondMovie = new Movie("The Beginnator");
        Assert.Equal("The Beginnator", secondMovie.Title);

        var movieBundle = new MovieBundle(firstMovie);
        Assert.Equal("Bundle of: Slow and Safe", movieBundle.Title);

        movieBundle.AddMovie(secondMovie);
        Assert.Equal("Bundle of: Slow and Safe, The Beginnator", movieBundle.Title);
    }

    /// <summary>
    /// Component
    /// </summary>
    /// <remarks>
    /// - Is the abstraction for all components, including composite ones.
    /// - Declares the interface for objects in the composition.
    /// </remarks>
    public interface IMovie
    {
        string Title { get; }
    }


    /// <summary>
    /// Leaf
    /// </summary>
    /// <remarks>
    /// - Represents leaf objects in the composition.
    /// - Implements all Component methods.
    /// </remarks>
    public class Movie(string title) : IMovie
    {
        public string Title => title;
    }

    /// <summary>
    /// Composite
    /// </summary>
    /// <remarks>
    /// - Represents a Component that has children.
    /// - Implements methods to manipulate children.
    /// - Implements all Component methods, generally by delegating them to its children.
    /// </remarks>
    public class MovieBundle(params IMovie[] movies) : IMovie
    {
        private readonly List<IMovie> _movies = [..movies];

        public string Title => $"Bundle of: {string.Join(", ", _movies.Select(movie => movie.Title))}";

        public void AddMovie(params IMovie[] movies)
        {
            _movies.AddRange(movies);
        }
    }
}