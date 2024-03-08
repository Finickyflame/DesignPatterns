namespace DesignPatterns.Behavioral;

public class Strategy
{
    [Fact]
    public void Execute()
    {
        SearchableCollection collection =
        [
            "John Doe",
            "Jane Doe",
            "Toby Chassuller",
            "Daphne Dordant",
            "Gilmore Spefable",
            "Natasha Froddond",
            "Percy Brerder"
        ];

        // Without strategy
        Assert.Empty(collection.Search("John Doe"));
        Assert.Empty(collection.Search("Jordan Nesk"));
        Assert.Empty(collection.Search("Do"));


        // With strategies
        collection.SetStrategy(new SearchStrategyWithEquals());
        Assert.Equal(["John Doe"], collection.Search("John Doe"));
        Assert.Empty(collection.Search("Jordan Nesk"));
        Assert.Empty(collection.Search("Do"));

        collection.SetStrategy(new SearchStrategyWithContains());
        Assert.Equal(["John Doe"], collection.Search("John Doe"));
        Assert.Empty(collection.Search("Jordan Nesk"));
        Assert.Equal(["John Doe", "Jane Doe", "Daphne Dordant", "Natasha Froddond"], collection.Search("Do"));
    }

    /// <summary>
    /// Strategy
    /// </summary>
    /// <remarks>
    /// Declares an interface common to all supported algorithms.
    /// Context uses this interface to call the algorithm defined by a Concrete Strategy
    /// </remarks>
    public interface ISearchStrategy
    {
        IEnumerable<string> Search(IEnumerable<string> collection, string criteria);
    }

    /// <summary>
    /// Context
    /// </summary>
    /// <remarks>
    /// - Is configured with a Concrete Strategy object.
    /// - Maintains a reference to a Strategy object.
    /// - May define an interface that lets Strategy access its data.
    /// </remarks>
    public class SearchableCollection : IEnumerable<string>
    {
        private readonly List<string> _collection = [];

        private ISearchStrategy Strategy { get; set; }
        
        public void Add(string value)
        {
            _collection.Add(value);
        }

        public IEnumerable<string> Search(string criteria) => Strategy?.Search(this, criteria) ?? Enumerable.Empty<string>();

        public void SetStrategy(ISearchStrategy strategy)
        {
            Strategy = strategy;
        }

        public IEnumerator<string> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();
    }

    /// <summary>
    /// Concrete Strategy
    /// </summary>
    /// <remarks>
    /// Implements the algorithm using the Strategy interface.
    /// </remarks>
    public class SearchStrategyWithEquals : ISearchStrategy
    {
        public IEnumerable<string> Search(IEnumerable<string> collection, string criteria)
        {
            return collection.Where(value => Equals(value, criteria));
        }

        private static bool Equals(string value, string criteria)
        {
            return value == criteria;
        }
    }

    /// <summary>
    /// Concrete Strategy
    /// </summary>
    /// <remarks>
    /// Implements the algorithm using the Strategy interface.
    /// </remarks>
    public class SearchStrategyWithContains : ISearchStrategy
    {
        public IEnumerable<string> Search(IEnumerable<string> collection, string criteria)
        {
            return collection.Where(value => Contains(value, criteria));
        }

        private static bool Contains(string value, string criteria)
        {
            return value.Contains(criteria, StringComparison.OrdinalIgnoreCase);
        }
    }
}