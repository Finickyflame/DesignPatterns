namespace DesignPatterns.Behavioral;

public partial class Iterator
{
    [Fact]
    public void Execute()
    {
        IAggregate<string> sentence = new Sentence("The quick brown fox jumps over the lazy dog.");
        IIterator<string> sentenceIterator = sentence.CreateIterator();

        // To iterate the words in the sentence:
        // while (sentenceIterator.HasNext())
        // {
        //     string word = sentenceIterator.Next();
        // }

        Assert.NotNull(sentenceIterator);
        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("The", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("quick", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("brown", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("fox", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("jumps", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("over", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("the", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("lazy", sentenceIterator.Next());

        Assert.True(sentenceIterator.HasNext());
        Assert.Equal("dog", sentenceIterator.Next());

        Assert.False(sentenceIterator.HasNext());


        IEnumerable<string> sentenceAsEnumerable = new SentenceAsEnumerable("The quick brown fox jumps over the lazy dog.");
        Assert.Equal(["The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog"], sentenceAsEnumerable);

        // To iterate over an Enumerable:
        //
        // using (IEnumerator<string> sentenceEnumerator = sentenceAsEnumerable.GetEnumerator())
        // {
        //     while (sentenceEnumerator.MoveNext())
        //     {
        //         string word = sentenceEnumerator.Current;
        //
        //     }
        // }
        //
        // or simply:
        //
        // foreach (string word in sentenceAsEnumerable)
        // {
        //    
        // }
    }

    /// <summary>
    /// Iterator
    /// </summary>
    /// <remarks>
    /// Defines an interface for accessing and traversing elements.
    /// </remarks>
    /// <typeparam name="T">
    /// Covariance (out) enables you to use a more derived type than originally specified.
    /// You can assign an instance of <see cref="IIterator{Derived}" /> to a variable of type <see cref="IIterator{Base}" />.
    /// </typeparam>
    public interface IIterator<out T>
    {
        bool HasNext();
        T Next();
    }

    /// <summary>
    /// Aggregate
    /// </summary>
    /// <remarks>
    /// Defines an interface for creating an Iterator object.
    /// </remarks>
    /// <typeparam name="T">
    /// Covariance (out) enables you to use a more derived type than originally specified.
    /// You can assign an instance of <see cref="IAggregate{Derived}" /> to a variable of type <see cref="IAggregate{Base}" />.
    /// </typeparam>
    public interface IAggregate<out T>
    {
        IIterator<T> CreateIterator();
    }

    /// <summary>
    /// Concrete Aggregate
    /// </summary>
    /// <remarks>
    /// Implements the Iterator creation interface to return an instance of the proper Concrete Iterator.
    /// </remarks>
    public class Sentence(string sentence) : IAggregate<string>
    {
        private string[] Words { get; } = SplitWords(sentence);

        public IIterator<string> CreateIterator() => new WordsIterator(this);

        /// <summary>
        /// Concrete Iterator
        /// </summary>
        /// <remarks>
        /// - Implements the Iterator interface.
        /// - Keeps track of the current position in the traversal of the aggregate.
        /// </remarks>
        private sealed class WordsIterator(Sentence sentence) : IIterator<string>
        {
            private int _index = 0;

            public bool HasNext() => sentence.Words.Length > _index;

            public string Next() => HasNext() ? sentence.Words[_index++] : null;
        }
    }

    /// <summary>
    /// Concrete Aggregate
    /// </summary>
    /// <remarks>
    /// In C#, IEnumerable is considered the equivalent of the Aggregate.
    /// Using this interface will allow you to use default features in C#, like foreach and linq expressions.
    /// </remarks>
    public class SentenceAsEnumerable(string sentence) : IEnumerable<string>
    {
        private string[] Words { get; } = SplitWords(sentence);

        /// <summary>
        /// Creates the IEnumerator object (Iterator).
        /// </summary>
        /// <returns>
        /// Generic Iterator,
        /// </returns>
        public IEnumerator<string> GetEnumerator() => new WordsEnumerator(this);

        /// <summary>
        /// The non-generic IEnumerable asks for an object iterator, we can simply return the same iterator that we already have
        /// implemented.
        /// </summary>
        /// <returns>
        /// Object Iterator
        /// </returns>
        /// <remarks>
        /// When you implement multiple interfaces and they contain the same properties/methods name but with different types,
        /// you will have to explicitly implement them.
        /// Writing the {InterfaceName}.{MethodName | PropertyName} is considered as an explicit implementation.
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Concrete Iterator
        /// </summary>
        /// <remarks>
        /// In C#, IEnumerator is the equivalent of the Iterator.
        /// </remarks>
        private sealed class WordsEnumerator : IEnumerator<string>
        {
            private readonly SentenceAsEnumerable _sentence;
            private int _index;

            public WordsEnumerator(SentenceAsEnumerable sentence)
            {
                _sentence = sentence;
                _index = 0;
                Current = default;
            }

            public bool MoveNext()
            {
                if (_sentence.Words.Length > _index)
                {
                    Current = _sentence.Words[_index++];
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
                Current = default;
            }

            public string Current { get; private set; }

            /// <summary>
            /// IEnumerator's interface asks for an object for the current Property, you can simply return our other property.
            /// </summary>
            object IEnumerator.Current => Current;

            public void Dispose()
            {
                // Nothing to dispose
            }
        }
    }

    private static string[] SplitWords(string sentence)
    {
        return WordExpression()
            .Matches(sentence ?? "")
            .Select(match => match.Value)
            .ToArray();
    }

    [GeneratedRegex("[^ .,']+")]
    private static partial Regex WordExpression();
}