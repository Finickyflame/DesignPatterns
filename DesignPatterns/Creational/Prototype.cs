using Xunit;

namespace DesignPatterns.Creational
{
    public class Prototype : DesignPattern
    {
        /// <summary>
        /// Client
        /// </summary>
        /// <remarks>
        /// Creates a new object by asking a prototype to clone itself.
        /// </remarks>
        public override void Execute()
        {
            var sheepCollection = new Sheep[]
            {
                new BlackSheep(),
                new WhiteSheep()
            };

            foreach (Sheep sheep in sheepCollection)
            {
                Sheep clone = sheep.Clone();
                clone.Shave();

                Assert.NotEqual(sheep.Shaved, clone.Shaved);
                Assert.Equal(sheep.HairColor, clone.HairColor);
            }
        }

        #region Definition

        /// <summary>
        /// Prototype
        /// </summary>
        /// <remarks>
        /// Declares an interface for cloning itself.
        /// </remarks>
        public abstract class Sheep
        {
            protected Sheep(string color)
            {
                this.HairColor = color;
            }

            public bool Shaved { get; private set; }

            public string HairColor { get; }

            public abstract Sheep Clone();

            public void Shave()
            {
                this.Shaved = true;
            }
        }

        #endregion

        #region Concrete Implementations

        /// <summary>
        /// Concrete Prototype
        /// </summary>
        /// <remarks>
        /// Implements an operation for cloning itself
        /// </remarks>
        public class BlackSheep : Sheep
        {
            public BlackSheep()
                : base("Black")
            {
            }

            public override Sheep Clone()
            {
                return new BlackSheep();
            }
        }

        /// <summary>
        /// Concrete Prototype
        /// </summary>
        /// <remarks>
        /// Implements an operation for cloning itself
        /// </remarks>
        public class WhiteSheep : Sheep
        {
            public WhiteSheep()
                : base("White")
            {
            }

            public override Sheep Clone()
            {
                return new WhiteSheep();
            }
        }

        #endregion
    }
}