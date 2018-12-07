using Xunit;

namespace DesignPatterns.Creational
{
    public class Prototype : DesignPattern
    {
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
        private abstract class Sheep
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

        #region Concrete Implementation

        private class BlackSheep : Sheep
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

        private class WhiteSheep : Sheep
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
