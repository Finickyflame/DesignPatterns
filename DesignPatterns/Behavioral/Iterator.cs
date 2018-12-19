using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class Iterator : DesignPattern
    {
        public override void Execute()
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
            Assert.Equal(new[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog" }, sentenceAsEnumerable);

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

        #region Definition

        /// <summary>
        /// Iterator
        /// </summary>
        /// <remarks>
        /// Defines an interface for accessing and traversing elements.
        /// </remarks>
        /// <typeparam name="T">
        /// Covariance (out) enables you to use a more derived type than originally specified.
        /// You can assign an instance of <see cref="IIterator{Derived}"/> to a variable of type <see cref="IIterator{Base}"/>.
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
        /// You can assign an instance of <see cref="IAggregate{Derived}"/> to a variable of type <see cref="IAggregate{Base}"/>.
        /// </typeparam>
        public interface IAggregate<out T>
        {
            IIterator<T> CreateIterator();
        }

        #endregion

        #region Concrete Implementation

        /// <summary>
        /// Concrete Aggregate
        /// </summary>
        /// <remarks>
        /// Implements the Iterator creation interface to return an instance of the proper Concrete Iterator.
        /// </remarks>
        public class Sentence : IAggregate<string>
        {
            public Sentence(string sentence)
            {
                this.Words = SplitWords(sentence);
            }

            private string[] Words { get; }

            public IIterator<string> CreateIterator()
            {
                return new WordsIterator(this);
            }

            /// <summary>
            /// Concrete Iterator
            /// </summary>
            /// <remarks>
            /// - Implements the Iterator interface.
            /// - Keeps track of the current position in the traversal of the aggregate.
            /// </remarks>
            private sealed class WordsIterator : IIterator<string>
            {
                private int _index;
                private readonly Sentence _sentence;

                public WordsIterator(Sentence sentence)
                {
                    this._sentence = sentence;
                    this._index = 0;
                }

                public bool HasNext()
                {
                    return this._sentence.Words.Length > this._index;
                }

                public string Next()
                {
                    return this.HasNext() ? this._sentence.Words[this._index++] : null;
                }
            }
        }

        /// <summary>
        /// Concrete Aggregate
        /// </summary>
        /// <remarks>
        /// In C#, IEnumerable is considered the equivalent of the Aggregate.
        /// Using this interface will allow you to use default features in C#, like foreach and linq expressions.
        /// </remarks>
        public class SentenceAsEnumerable : IEnumerable<string>
        {
            public SentenceAsEnumerable(string sentence)
            {
                this.Words = SplitWords(sentence);
            }

            private string[] Words { get; }

            /// <summary>
            /// Creates the IEnumerator object (Iterator).
            /// </summary>
            /// <returns>
            /// Generic Iterator, 
            /// </returns>
            public IEnumerator<string> GetEnumerator()
            {
                return new WordsEnumerator(this);
            }

            /// <summary>
            /// The non-generic IEnumerable asks for an object iterator, we can simply return the same iterator that we already have implemented.
            /// </summary>
            /// <returns>
            /// Object Iterator
            /// </returns>
            /// <remarks>
            /// When you implement multiple interfaces and they contain the same properties/methods name but with different types,
            /// you will have to explicitly implement them.
            /// Writing the {InterfaceName}.{MethodName | PropertyName} is considered as an explicit implementation.
            /// </remarks>
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            /// <summary>
            /// Concrete Iterator
            /// </summary>
            /// <remarks>
            /// In C#, IEnumerator is the equivalent of the Iterator.
            /// </remarks>
            private sealed class WordsEnumerator : IEnumerator<string>
            {
                private int _index;
                private readonly SentenceAsEnumerable _sentence;

                public WordsEnumerator(SentenceAsEnumerable sentence)
                {
                    this._sentence = sentence;
                    this._index = 0;
                    this.Current = default(string);
                }

                public bool MoveNext()
                {
                    if (this._sentence.Words.Length > this._index)
                    {
                        this.Current = this._sentence.Words[this._index++];
                        return true;
                    }
                    return false;
                }

                public void Reset()
                {
                    this._index = 0;
                    this.Current = default(string);
                }

                public string Current { get; private set; }

                /// <summary>
                /// IEnumerator's interface asks for an object for the current Property, you can simply return our other property.
                /// </summary>
                object IEnumerator.Current => this.Current;

                public void Dispose()
                {
                    // Nothing to dispose
                }
            }
        }

        #endregion

        private static readonly Regex WordExpression = new Regex("[^ .,']+");

        private static string[] SplitWords(string sentence)
        {
            return WordExpression.Matches(sentence ?? "")
                                 .Select(match => match.Value)
                                 .ToArray();
        }
    }
}
