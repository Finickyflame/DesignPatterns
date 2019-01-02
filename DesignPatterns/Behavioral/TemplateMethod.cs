using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace DesignPatterns.Behavioral
{
    public class TemplateMethod : DesignPattern
    {
        public override void Execute()
        {
            OrderedCollection ascendingCollection = new AscendingCollection
            {
                3,
                4,
                2,
                8,
                1,
                5,
                3,
                6,
                9,
                7
            };
            Assert.Equal(new[] { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9 }, ascendingCollection);

            OrderedCollection descendingCollection = new DescendingCollection
            {
                3,
                4,
                2,
                8,
                1,
                5,
                3,
                6,
                9,
                7
            };
            Assert.Equal(new[] { 9, 8, 7, 6, 5, 4, 3, 3, 2, 1 }, descendingCollection);
        }

        /// <summary>
        /// Abstract class
        /// </summary>
        /// <remarks>
        /// - Defines abstract primitive operations that concrete subclasses define to implement steps of an algorithm.
        /// - Implements a template method defining the skeleton of an algorithm.
        /// - The template method calls primitive operations as well as operations defined in AbstractClass or those of other objects.
        /// </remarks>
        public abstract class OrderedCollection : IEnumerable<int>
        {
            private readonly List<int> _collection;


            protected OrderedCollection()
            {
                this._collection = new List<int>();
            }

            /// <summary>
            /// Template Method
            /// </summary>
            /// <param name="value"></param>
            public void Add(int value)
            {
                int index = this.FindIndex(this._collection, value);
                this._collection.Insert(index, value);
            }

            public IEnumerator<int> GetEnumerator()
            {
                return this._collection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this._collection).GetEnumerator();
            }

            /// <summary>
            /// Primitive Operation
            /// </summary>
            /// <param name="source"/>
            /// <param name="value"/>
            /// <returns></returns>
            protected abstract int FindIndex(IReadOnlyList<int> source, int value);
        }

        /// <summary>
        /// Concrete Class
        /// </summary>
        /// <remarks>
        /// Implements the primitive operations to carry out subclass-specific steps of the algorithm.
        /// </remarks>
        public class AscendingCollection : OrderedCollection
        {
            /// <inheritdoc />
            protected override int FindIndex(IReadOnlyList<int> source, int value)
            {
                for (int index = source.Count; index > 0; index--)
                {
                    if (value >= source[index - 1])
                    {
                        return index;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// Concrete Class
        /// </summary>
        /// <remarks>
        /// Implements the primitive operations to carry out subclass-specific steps of the algorithm.
        /// </remarks>
        public class DescendingCollection : OrderedCollection
        {
            /// <inheritdoc />
            protected override int FindIndex(IReadOnlyList<int> source, int value)
            {
                for (int index = 0; index < source.Count; index++)
                {
                    if (value >= source[index])
                    {
                        return index;
                    }
                }
                return source.Count;
            }
        }
    }
}